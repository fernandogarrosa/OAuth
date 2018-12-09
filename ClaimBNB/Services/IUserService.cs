using ClaimBNB.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBNB.Services
{
    public interface IUserService
    {
        Task<UserForListDto> GetUserById(int id);
        Task<bool> ExistUserByName(string name);
        Task<bool> ExistUserById(int id);
        Task<bool> DeleteUserById(int id);
    }
}
