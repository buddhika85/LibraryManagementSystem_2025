using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// A parent class containing all generic methods used by account, staff, member controllers
    /// members are visible only to child classes in the same assembly
    /// </summary>
    public class UserController : BaseApiController
    {
        private protected readonly SignInManager<AppUser> signInManager;
        private protected readonly UserManager<AppUser> userManager;
        private protected readonly IUserService userService;
        private protected readonly IMapper mapper;

        private protected UserController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            IUserService userService, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
            this.mapper = mapper;
        }


        #region HelpersForChildClasses

        private protected async Task<IActionResult> UpdateUserAsync(string username, UserUpdateDto updateDto, AppUser user)
        {
            CustomMapper.MapToUserFromUpdateDto(user, updateDto);
            // Save changes to the database using UserManager
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                AddErrorsToModelState(result);
                return ValidationProblem();
            }
            else
            {
                if (updateDto.Address != null)
                {
                    var resultDto = await userService.UpdateAddressOfUser(username, updateDto.Address);
                    if (resultDto.IsSuccess)
                        return NoContent();

                    return StatusCode(500, $"An error occurred while updating the user with username {username} - Address section {updateDto.Address}.");
                }
            }
            return NoContent();
        }

        #endregion
    }
}
