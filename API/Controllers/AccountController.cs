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
        private readonly IMapper mapper;

        public AccountController(SignInManager<AppUser> signInManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }

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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAuthState()
        {
            // anonymous will return false
            return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
        }

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
