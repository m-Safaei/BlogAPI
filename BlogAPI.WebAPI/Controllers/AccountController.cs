﻿using BlogAPI.Core.Domain.Entities;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<ApplicationUser> userManager
                                , SignInManager<ApplicationUser> signInManager
                                , RoleManager<ApplicationRole> roleManager
                                , IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userService = userService;
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

                return Ok(result);
            }

            return StatusCode(500, result);
        }
    }
}
