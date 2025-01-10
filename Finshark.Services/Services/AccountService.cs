using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Finshark.Core.Interfaces.Service;
using Microsoft.AspNetCore.Identity;

namespace Finshark.Services.Services
{
    public class AccountService : IAccountService
    {
         private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }
        public async Task<object> LoginAsync(LoginDTO loginDTO)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDTO.Username.ToLower());

            if (user == null) return null;

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDTO.Password, false); 

            if (!result.Succeeded) return "Password incorrect";

            return new NewUserDTO 
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                };
        }
public async Task<NewUserDTO> RegisterAsync(RegisterDTO registerDto)
{
    var appUser = new AppUser
    {
        UserName = registerDto.Username,
        Email = registerDto.Email,
    };

    var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

    if (!createdUser.Succeeded)
    {
        return null; // Handle error logging or custom error handling here if needed.
    }

    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

    if (!roleResult.Succeeded)
    {
        return null; // Handle error logging or custom error handling here if needed.
    }

    return new NewUserDTO
    {
        Username = registerDto.Username,
        Email = registerDto.Email,
        Token = _tokenService.CreateToken(appUser),
    };
}

    }
}