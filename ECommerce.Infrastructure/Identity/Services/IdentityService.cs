using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Identity;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IdentityUserResult>.Fail(Error.NotFound("User.NotFound", $"User wil email {email} is not found"));
            else
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email!, user.UserName!));

        }

        public async Task<Result<bool>> CheckUserPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<bool>.Fail(Error.NotFound("User.NotFound", $"User wil email {email} is not found"));
            else
                return Result<bool>.Ok(await _userManager.CheckPasswordAsync(user, password));


        }

        public async Task<Result<IdentityUserResult>> RegisterUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName
            };

            //we send the password in the CreateAsync Method so that it can be hashed.
            var res = await _userManager.CreateAsync(user, registerDto.Password);

            if (!res.Succeeded)
            {
                var errors = res.Errors.Select(x => new Error(x.Code, x.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }


            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.UserName, user.Email));
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound("User.NotFound", $"User wil email {email} is not found"));

            var res = await _userManager.GetRolesAsync(user);
            return Result<IReadOnlyList<string>>.Ok(res.ToList());
        }
    }
}
