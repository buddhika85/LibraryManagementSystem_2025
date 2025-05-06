using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
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
            if (!await unitOfWork.SaveAllAsTransactionAsync())
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

        public async Task<ResultDto> UpdateAddressOfUser(string username, AddressDto address)
        {
            var resultDto = new ResultDto();
            int addressId = await userRepository.FindAddressIdByUsernameAsync(username);
            var addressModel = await addressRepository.GetByIdAsync(addressId);
            if (addressModel == null)
            {
                resultDto.ErrorMessage = $"Address does not exisit for username  {username}";
                return resultDto;
            }

            mapper.Map(address, addressModel);
            addressRepository.Update(addressModel);

            var result = await unitOfWork.SaveAllAsTransactionAsync();
            if (!result)
            {
                resultDto.ErrorMessage = $"Updating address for username {username} is unsuccessful";
                return resultDto;
            }
           
            return resultDto;
        }

        public async Task<UserInfoDto?> GetUserByRoleAndEmailAsync(string email, UserRoles filterRole)
        {
            var user = await userRepository.GetUserByRoleAndEmailAsync(email, filterRole);
            if (user == null)
            {
                return null;
            }

            return mapper.Map<UserInfoDto>(user);
        }
    }
}
