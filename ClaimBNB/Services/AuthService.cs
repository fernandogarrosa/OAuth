using AutoMapper;
using ClaimBNB.Dtos;
using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBNB.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<UserForListDto> RegisterUser(UserForRegisterDto userForRegisterDto)
        {
            User userToCreate = mapper.Map<User>(userForRegisterDto);
            var result = await userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded)          
               return  mapper.Map<UserForListDto>(userToCreate);

            return null;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }        
    }
}
