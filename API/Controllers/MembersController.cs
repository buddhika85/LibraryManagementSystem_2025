using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
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
        private protected MembersController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IUserService userService, IMapper mapper) : base(signInManager, userManager, userService, mapper)
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
        /// Returns all staff. Admin only privilaged.
        /// </summary>
        /// <returns>Returns all staff</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("allStaff")]
        public async Task<ActionResult<IReadOnlyList<UsersListDto>>> GetStaffAsync()
        {
            return Ok(await userService.GetStaffAsync());
        }


        /// <summary>
        /// Admin can edit staff/members
        /// </summary>
        /// <param name="username">username / email</param>
        /// <param name="updateDto">update information</param>
        /// <returns>returns update status</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("updateUser/{username}")]
        public async Task<IActionResult> UpdateStaffOrMember(string username, UserUpdateDto updateDto)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest($"Staff, Member with such username does not exists : {username}");
            }

            var roles = await userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any() ||
                (roles[0] != UserRoles.Member.ToString() && roles[0] != UserRoles.Staff.ToString()))
            {
                return BadRequest($"Staff, Member with such username does not exists : {username}");
            }

            return await UpdateUserAsync(username, updateDto, user);
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

        /// <summary>
        /// Admin can delete staff/member user records
        /// </summary>
        /// <returns>Deletion status</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteByUsername(string username)
        {
            var userToDelete = await userManager.FindByNameAsync(username);
            if (userToDelete == null)
            {
                return BadRequest($"User with such username does not exists : {username}");
            }

            ResultDto result = await userService.DeleteUserAsync(username);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, $"An error occurred while deleting the user with username {username}.");
            }
        }



        
    }
}
