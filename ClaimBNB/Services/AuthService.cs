using AutoMapper;
using ClaimBNB.Dtos;
using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClaimBNB.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;

        public AuthService(IMapper mapper, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<UserForListDto> RegisterUser(UserForRegisterDto userForRegisterDto)
        {
            User userToCreate = mapper.Map<User>(userForRegisterDto);
            var result = await userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded)          
               return  mapper.Map<UserForListDto>(userToCreate);

            return null;
        }

        public async Task<UserForListDto> Login(string userName, string password)
        {
            User user = await userManager.FindByNameAsync(userName);
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                User appUser = await userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());

                UserForListDto userForListDto = mapper.Map<UserForListDto>(appUser);

                return userForListDto;
            }
            return null;
        }

        public async Task<string> GenerateJwtToken(UserForListDto userForListDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userForListDto.Id.ToString()),
                new Claim(ClaimTypes.Name, userForListDto.UserName)
            };

            User user = mapper.Map<User>(userForListDto);
            var roles = await userManager.GetRolesAsync(user);

            foreach(string role in roles)            
                claims.Add(new Claim(ClaimTypes.Role, role));
            

            var key = new SymmetricSecurityKey(Encoding.UTF8.
                                GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }       
    }
}
