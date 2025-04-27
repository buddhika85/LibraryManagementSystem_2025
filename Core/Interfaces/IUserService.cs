using Core.DTOs;

namespace Core.Interfaces
{
    public interface IUserService
    {        
        public Task<UsersListDto> GetMembersAsync();
        public Task<UsersListDto> GetStaffAsync();
        Task<ResultDto> DeleteAddressAsync(int id);
        Task<ResultDto> DeleteUserAsync(string username);
    }
}
