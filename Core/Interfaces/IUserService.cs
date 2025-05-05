using Core.DTOs;
using Core.Enums;


namespace Core.Interfaces
{
    public interface IUserService
    {        
        Task<UsersListDto> GetMembersAsync();
        Task<UsersListDto> GetStaffAsync();

        
        Task<ResultDto> DeleteAddressAsync(int id);
        Task<ResultDto> DeleteUserAsync(string username);
        Task<ResultDto> UpdateAddressOfUser(string username, AddressDto address);


        Task<UserInfoDto?> GetUserByRoleAndEmailAsync(string email, UserRoles filterRole);
    }
}
