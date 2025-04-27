using API.Extensions;
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
    [Authorize]
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, 
            IUserService userService, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
            this.mapper = mapper;
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


        // TO DO: Admin can edit staff/members
        // TO DO: Staf can edit members

        // TO DO: Admin can delete staff/members
        
        // TO DO: Staf can delete members

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await signInManager.PasswordSignInAsync(
                user, loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok("Login successful");
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

            return Ok(mapper.Map<UserInfoDto>(user));
        }


        /// <summary>
        /// Are there a user logged in from client (browser) the request came from?
        /// </summary>
        /// <returns>Logged in or not</returns>
        [AllowAnonymous]
        [HttpGet]
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


        #region Helpers
        private async Task<IActionResult> RegisterHelper(RegisterDto registerDto)
        {
            var userExists = await userManager.FindByNameAsync(registerDto.Email) != null;
            if (userExists)
            {
                return BadRequest($"User with same username already exists : {registerDto.Email}");
            }

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

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }
        #endregion 
    }
}
