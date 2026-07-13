using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> CheckUserPasswordAsync(string email, string password, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> RegisterUserAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default);
    }
}
