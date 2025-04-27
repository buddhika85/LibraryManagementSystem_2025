using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }             

        public async Task<IReadOnlyList<AppUser>> GetMembersAsync()
        {
            return await FindUsersByRoleAsync(UserRoles.Member);
        }        

        public async Task<IReadOnlyList<AppUser>> GetStaffAsync()
        {
            return await FindUsersByRoleAsync(UserRoles.Staff);
        }

        private async Task<IReadOnlyList<AppUser>> FindUsersByRoleAsync(UserRoles filterRole)
        {
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == filterRole.ToString());
            var userRoles = context.UserRoles;
            if (role == null)
            {
                throw new InvalidOperationException("Role not found.");
            }

            return await context.Users.Include(x => x.Address)
                .Where(x => userRoles.Any(r => r.UserId == x.Id && r.RoleId == role.Id))                
                .ToListAsync();
        }
    }
}
