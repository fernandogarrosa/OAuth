using AutoMapper;
using Data.Models;
using Data.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClaimBNB.Dtos;

namespace ClaimBNB.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;
        private readonly IMapper mapper;

        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;            
        }

        public async Task<bool> DeleteUserById(int id)
        {
            return await userRepository.DeleteAsync(id) > 0;
        }

        public async Task<bool> ExistUserById(int id)
        {
            return await userRepository.GetTable().AnyAsync(u => u.Id == id);
        }

        public async Task<bool> ExistUserByName(string name)
        {
            return await userRepository.GetTable().AnyAsync(u => u.UserName == name);
        }

        public async Task<UserForListDto> GetUserById(int id)
        {
            return mapper.Map<UserForListDto>(await userRepository.GetAsync(id));
        }
    }
}
