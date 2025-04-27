using AutoMapper;
using Core.DTOs;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private IMapper mapper;
        private IUnitOfWork unitOfWork;
        private IUserRepository userRepository;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.UserRepository;
        }

        public async Task<UsersListDto> GetMembersAsync()
        {
            var members = await userRepository.GetMembersAsync();
            UsersListDto dto = new();
            if (members != null && members.Any())
            {
                dto.UsersList = mapper.Map<IReadOnlyList<MemberUserDisplayDto>>(members);
            }
            return dto;
        }

        public async Task<UsersListDto> GetStaffAsync()
        {
            var staffUsers = await userRepository.GetStaffAsync();
            UsersListDto dto = new();
            if (staffUsers != null && staffUsers.Any())
            {
                dto.UsersList = mapper.Map<IReadOnlyList<StaffUserDisplayDto>>(staffUsers);
            }
            return dto;
        }
    }
}
