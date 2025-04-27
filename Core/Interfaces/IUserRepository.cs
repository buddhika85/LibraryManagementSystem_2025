using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<IReadOnlyList<AppUser>> GetMembersAsync();
        public Task<IReadOnlyList<AppUser>> GetStaffAsync();
    }
}
