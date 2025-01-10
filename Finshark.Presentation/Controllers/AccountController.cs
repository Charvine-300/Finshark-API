using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Finshark.Core.Interfaces.Service;
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

        private readonly IAccountService _accountService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IAccountService accountService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
            _accountService = accountService;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userLogin = await _accountService.LoginAsync(loginDTO);

            if (userLogin == null) return Unauthorized("Invalid username");

            else if (userLogin.ToString() == "Password incorrect") return Unauthorized("Password incorrect");

            return Ok(userLogin);
        }

[HttpPost]
[Route("register")]
public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        var newUser = await _accountService.RegisterAsync(registerDto);

        if (newUser == null)
        {
            return StatusCode(500, "An error occurred while creating the user.");
        }

        return Ok(newUser);
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}

    }
}