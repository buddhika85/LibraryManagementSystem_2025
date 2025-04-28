using Core.DTOs;
using Core.Entities;

namespace Infrastructure.Helpers
{
    public static class CustomMapper
    {
        public static void MapToUserFromUpdateDto(AppUser user, UserUpdateDto dto)
        {
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
        }
    }
}
