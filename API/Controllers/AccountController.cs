using API.Extensions;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Identity and account/profile management
    /// </summary>
    [Authorize]
    public class AccountController : UserController
    {
        private protected AccountController(SignInManager<AppUser> signInManager, 
            UserManager<AppUser> userManager, IUserService userService, IMapper mapper) : base(signInManager, userManager, userService, mapper)
        {
        }


        /// <summary>
        /// Member registration.
        /// Any role and guest can create/register members
        /// </summary>
        /// <param name="registerDto">User, address information and role to assign</param>
        /// <returns>Returns registration request was successful or not</returns>
        [AllowAnonymous]
        [HttpPost("registerMember")]
        public async Task<IActionResult> RegisterMember(MemberRegisterDto memberRegisterDto)
        {
            if (memberRegisterDto.Role != UserRoles.Member)
            {
                return BadRequest("Role must be Member for MemberRegisterDto.");
            }
            return await RegisterHelper(memberRegisterDto);
        }        

        /// <summary>
        /// Creates a new user along and assigns the requested role. 
        /// Admin Only - can create/register staff, members
        /// </summary>
        /// <param name="registerDto">User, address information and role to assign</param>
        /// <returns>Returns registration request was successful or not</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            return await RegisterHelper(registerDto);
        }

        
               

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await signInManager.PasswordSignInAsync(
                user, loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful" });
            }

            return Unauthorized("Invalid credentials");
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }

        /// <summary>
        /// Returns logged in users information
        /// </summary>
        /// <returns>UserInfoDto containing logged in users information</returns>
        [AllowAnonymous]
        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false)
                return NoContent();     // anonymous will return NoContent

            var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);
            if (user == null)
                return Unauthorized();

           
            var roles = await userManager.GetRolesAsync(user);
            if (roles == null)
                return Unauthorized();

            var userDto = mapper.Map<UserInfoDto>(user);
            userDto.Role = Enum.Parse<UserRoles>(roles[0]);
            return Ok(userDto);
        }


        /// <summary>
        /// Are there a user logged in from client (browser) the request came from?
        /// </summary>
        /// <returns>Logged in or not</returns>
        [AllowAnonymous]
        [HttpGet("auth-status")]
        public ActionResult GetAuthState()
        {
            // anonymous will return false
            return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
        }

        /// <summary>
        /// Updates or creates an address for the logged in user
        /// </summary>
        /// <param name="addressDto">Address information</param>
        /// <returns>Returns the created address</returns>
        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<AddressDto>> CreateOrUpdateAddress(AddressDto addressDto)
        {
            var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);
            if (user.Address == null)
            {
                user.Address = mapper.Map<Address>(addressDto);
            }
            else
            {
                user.Address = mapper.Map(addressDto, user.Address);
            }
            var result = await signInManager.UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Problem updating user address");
            }
            return Ok(mapper.Map<AddressDto>(user.Address));
        }


        /// <summary>
        /// Authorised users updating their own profiles
        /// </summary>
        /// <param name="username">username / email</param>
        /// <param name="updateDto">update information</param>
        /// <returns>returns update status</returns>
        [Authorize]
        [HttpPut("updateProfile/{username}")]
        public async Task<IActionResult> UpdateProfile(string username, UserUpdateDto updateDto)
        {
            var loggedInUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            if (loggedInUsername != username)
                 return BadRequest($"{username} is not logged in to update profile");

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest($"Member with such username does not exists : {username}");
            }

            return await UpdateUserAsync(username, updateDto, user);           
        }
                
        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var loggedInUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            if (loggedInUsername != dto.Username)
                return BadRequest($"{dto.Username} is not logged in to update profile");

            var user = await userManager.FindByNameAsync(dto.Username);
            if (user == null)
            {
                return BadRequest($"Member with such username does not exists : {dto.Username}");
            }

            var result = await userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (result.Succeeded)
                return NoContent();

            AddErrorsToModelState(result);
            return ValidationProblem();
        }




        #region Helpers

       

        private async Task<IActionResult> RegisterHelper(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email,
                Address = mapper.Map<Address>(registerDto.Address),
            };

            // add user along with address
            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

            // add user to Role
            if (result.Succeeded)
            {
                result = await signInManager.UserManager.AddToRoleAsync(user, registerDto.Role.ToString());
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

            return Ok();
        }

       
        #endregion 
    }
}
