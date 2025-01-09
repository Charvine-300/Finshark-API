using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    
    public class AccountController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDTO.Username.ToLower());

            if (user == null) {
                return Unauthorized("Invalid username");
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded) return Unauthorized("Password incorrect");

            return Ok(
                new NewUserDTO 
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                }
            );
        }

        [HttpPost] // Register User
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded) {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User"); // Assign role to user

                    if (roleResult.Succeeded) {
                        return Ok(
                            new NewUserDTO
                            {
                                Username = registerDto.Username,
                                Email = registerDto.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    } else {
                        return StatusCode(500, roleResult.Errors);
                    }
                } else {
                    return StatusCode(500, createdUser.Errors);
                }
            } catch (Exception err) {
                return StatusCode(500, err);
            }
        }
    }
}