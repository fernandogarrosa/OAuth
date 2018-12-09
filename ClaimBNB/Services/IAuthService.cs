using ClaimBNB.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBNB.Services
{
    public interface IAuthService
    {
        Task<UserForListDto> RegisterUser(UserForRegisterDto userForRegisterDto);
        Task<UserForListDto> Login(string userName, string password);      
    }
}
