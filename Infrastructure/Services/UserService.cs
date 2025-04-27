using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private IMapper mapper;
        private IUnitOfWork unitOfWork;
        private IUserRepository userRepository;
        private IGenericRepository<Address> addressRepository;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.UserRepository;
            this.addressRepository = unitOfWork.AddressRepository;
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
        public async Task<ResultDto> DeleteAddressAsync(int id)
        {
            var result = new ResultDto();
            var addressToDelete = await addressRepository.GetByIdAsync(id);
            if (addressToDelete != null)
                this.addressRepository.Remove(addressToDelete);
            if (!await unitOfWork.SaveAllAsync())
            {
                result.ErrorMessage = "Could not delete the address.";
            }
            return result;
        }

        public async Task<ResultDto> DeleteUserAsync(string username)
        {
            if (await userRepository.DeleteUserAsync(username))
            {
                return new ResultDto();
            }
            return new ResultDto { ErrorMessage = $"User with username {username} deletion unsuccessful"};
        }
    }
}
