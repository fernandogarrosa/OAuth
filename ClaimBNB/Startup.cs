using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ClaimBNB.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ClaimBNB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddEntityFrameworkWithSqlite(Configuration);

            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                    .AddJwtBearer(options => {
                                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                                        {
                                            ValidateIssuerSigningKey = true,
                                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                                                                Configuration.GetSection("AppSettings:Token").Value)),
                                            ValidateIssuer = false,
                                            ValidateAudience = false
                                        };
                                    });

            services.AddMvc(
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().
            //                    RequireAuthenticatedUser().
            //                    Build();

            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<Seed>();

            services.AddAutoMapper();            
            services.AddCors();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.EnsureDeletedAsync().Wait();
                context.Database.EnsureCreatedAsync().Wait();
                // context.Initialize();
                seeder.SeedUsers();
            }
        }
    }
}
