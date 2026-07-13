using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;

        public AuthenticationService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            //Get user by email
            var userResult = await _identityService.FindUserByEmailAsync(loginDto.Email, ct);
            if (!userResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);

            //Verify Password
            var passwordResult = await _identityService.CheckUserPasswordAsync(loginDto.Email, loginDto.Password);
            
            //if CheckUserPassword returns Fail
            if (!passwordResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);

            //if CheckUserPassword returns Ok but Value is false
            if(!passwordResult.Value)
            {
                return Result<UserDto>.Fail(Error.Unauthorized("Invalid.Data", "Invalid Email Or Password"));
            }

            //Generate JWT Token


            //Return result 
            return new UserDto()
            {
                Email = loginDto.Email,
                DisplayName = userResult.Value.DisplayName,
                Token = "TODO"
            };

        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var userRes = await _identityService.RegisterUserAsync(registerDto, ct);
            if (!userRes.IsSuccess)
            {
                return Result<UserDto>.Fail(userRes.Errors);
            }
            var user = userRes.Value;

            return Result<UserDto>.Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "TODO"
            });
            
        }
    }
}
