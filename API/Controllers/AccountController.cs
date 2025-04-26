using API.Extensions;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates a new user along and assigns the requested role. 
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns>Returns registration request was successful or not</returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                Address = mapper.Map<Address>(registerDto.Address)
            };

            // add user along with address
            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

            // add user to Role
            if (result.Succeeded)
            {
                result = await signInManager.UserManager.AddToRoleAsync(user, registerDto.Role.ToString());
            }

            if (!result.Succeeded)
            {
                // return BadRequest(result.Errors);
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return Ok();
        }

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
        public async Task<ActionResult> Logout()
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
        public async Task<ActionResult> GetUserInfo()
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
    }
}
