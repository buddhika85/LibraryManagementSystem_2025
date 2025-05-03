using Core.Enums;

namespace Core.DTOs
{
    public class UsersListDto
    {
        public IReadOnlyList<UserDisplayDto> UsersList { get; set; } = new List<UserDisplayDto>();
        public int Count => UsersList.Count;
    }

    public class UserDisplayDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }         
        public required string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public required string Email { get; set; }         
        public required string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
        public string ActiveStr => IsActive ? "Yes" : "No";

        public virtual UserRoles Role { get; init; }
        public string RoleStr => Role.ToString();


        public required AddressDto Address { get; set; }
        public string AddressStr => Address.ToString();
    }

    public class MemberUserDisplayDto : UserDisplayDto
    {        
        public override UserRoles Role 
        { 
            get => base.Role;
            init
            {               
                base.Role = UserRoles.Member;
            }
        }
    }

    public class StaffUserDisplayDto : UserDisplayDto
    {
        public override UserRoles Role
        {
            get => base.Role;
            init
            {
                base.Role = UserRoles.Staff;
            }
        }
    }
}
