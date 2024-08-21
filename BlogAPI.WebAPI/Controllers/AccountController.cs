using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.DTO;
using BlogAPI.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AccountController(SignInManager<ApplicationUser> signInManager
                                , IUserService userService
                                ,IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userService.IsPhoneNumberAlreadyRegistered(registerDto.PhoneNumber))
                return BadRequest("This Phone number has already registered");

            var result = await _userService.AddUser(registerDto);
            if (result.GetType() == typeof(ApplicationUser))
            {
                //sign-In:
                await _signInManager.SignInAsync((ApplicationUser)result, isPersistent: false);
                var authenticationResponse = await _jwtService.CreateJwtToken(registerDto.PhoneNumber);
                return Ok(authenticationResponse);
            }

            return StatusCode(500, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginDto.PhoneNumber
                , loginDto.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                LoginResponseDto? user = await _userService.GetUserByPhoneNumber(loginDto.PhoneNumber);
                if (user == null) return NoContent();

                var authenticationResponse = await _jwtService.CreateJwtToken(loginDto.PhoneNumber);
                return Ok(authenticationResponse);
            }

            return Unauthorized("Invalid Phone number or password");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }
    }
}
