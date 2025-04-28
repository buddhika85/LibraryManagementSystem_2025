using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces
{
    public interface IUserService
    {        
        public Task<UsersListDto> GetMembersAsync();
        public Task<UsersListDto> GetStaffAsync();
        Task<ResultDto> DeleteAddressAsync(int id);
        Task<ResultDto> DeleteUserAsync(string username);
        Task<ResultDto> UpdateAddressOfUser(string username, AddressDto address);
    }
}
