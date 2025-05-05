using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{    
    /// <summary>
    /// Member users management
    /// </summary>
    public class MembersController : UserController
    {
        public MembersController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IUserService userService, IMapper mapper) : base(signInManager, userManager, userService, mapper)
        {
        }


        /// <summary>
        /// Returns all members. Admin and Staff privilaged.
        /// </summary>
        /// <returns>Returns all members</returns>
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("allMembers")]
        public async Task<ActionResult<IReadOnlyList<UsersListDto>>> GetMembersAsync()
        {
            return Ok(await userService.GetMembersAsync());
        }





        /// <summary>
        /// Activate or Deactivate Members
        /// Admin and Staff can deactivate members
        /// </summary>
        /// <param name="username">username of member</param>
        /// <returns>Sttaus of de/activation</returns>
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("activateDeactivateMembers/{username}")]
        public async Task<IActionResult> ActivateDeactivateMembers(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest($"Member with such username does not exists : {username}");
            }

            var roles = await userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any() ||
                roles[0] != UserRoles.Member.ToString())
            {
                return BadRequest($"Member with such username does not exists : {username}");
            }
            // Toggle IsActive property
            user.IsActive = !user.IsActive;

            // Save changes to the database using UserManager
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update user status.");
            }
            return NoContent();
        }



        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("getMemberForEdit/{email}")]
        public async Task<ActionResult<UserInfoDto>> GetMemberForEdit(string email)
        {
            var user = await userService.GetUserByRoleAndEmailAsync(email, UserRoles.Member);
            if (user == null)
                return BadRequest($"user with such email {email} does not exists");           

            var userDto = mapper.Map<UserInfoDto>(user);
            userDto.Role = UserRoles.Member;
            return Ok(userDto);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("addMember")]
        public async Task<ActionResult<ResultDto>> AddMember(InsertUpdateUserDto userDto)
        {
            var user = mapper.Map<AppUser>(userDto);        
            var randomPassword = PasswordGenerator.GeneratePassword(10);
            // add user along with address
            var result = await signInManager.UserManager.CreateAsync(user, randomPassword);

            // add user to Role
            if (result.Succeeded)
            {
                result = await signInManager.UserManager.AddToRoleAsync(user, UserRoles.Member.ToString());
            }
            else
            {
                AddErrorsToModelState(result);
                return ValidationProblem();
            }

            if (!result.Succeeded)
            {
                AddErrorsToModelState(result);
                return ValidationProblem();
            }

            return Ok(new ResultDto());
        }



        /// <summary>
        /// Admin, Staff updating member
        /// </summary>
        /// <param name="username">username / email</param>
        /// <param name="updateDto">update information</param>
        /// <returns>returns update status</returns>
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("updateMember/{username}")]
        public async Task<IActionResult> UpdateMember(string username, UserUpdateDto updateDto)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest($"Member with such username does not exists : {username}");
            }

            var roles = await userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any() ||
                roles[0] != UserRoles.Member.ToString())
            {
                return BadRequest($"Member with such username does not exists : {username}");
            }

            return await UpdateUserAsync(username, updateDto, user);
        }

    }
}
