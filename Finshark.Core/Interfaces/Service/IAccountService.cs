using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;

namespace Finshark.Core.Interfaces.Service
{
    public interface IAccountService
    {
    Task<object> LoginAsync(LoginDTO loginDTO);

    Task<NewUserDTO> RegisterAsync(RegisterDTO registerDto);
    }
}