using Core.DTOs;

namespace Core.Interfaces
{
    public interface IUserService
    {
        public Task<UsersListDto> GetMembersAsync();
        public Task<UsersListDto> GetStaffAsync();
    }
}
