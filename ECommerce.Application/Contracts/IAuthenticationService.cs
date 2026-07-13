using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IAuthenticationService
    {
        //Login (take email + password) -> (return token, email, and display name in DTO)
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default);

        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
    }
}
