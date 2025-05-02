using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Staff users management
    /// </summary>
    public class StaffController : UserController
    {
        public StaffController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IUserService userService, IMapper mapper) : base(signInManager, userManager, userService, mapper)
        {
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
