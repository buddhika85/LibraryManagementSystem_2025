using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        
        public Task<IReadOnlyList<AppUser>> GetMembersAsync();

        
        public Task<IReadOnlyList<AppUser>> GetStaffAsync();

        Task<bool> DeleteUserAsync(string username);
        Task<int> FindAddressIdByUsernameAsync(string username);


        public Task<AppUser?> GetUserByRoleAndEmailAsync(string email, UserRoles filterRole);
    }
}
