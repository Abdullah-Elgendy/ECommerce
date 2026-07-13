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
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
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
            var roles = await _identityService.GetUserRolesAsync(userResult.Value.Email, ct);
            var token = _tokenService.CreateToken(userResult.Value.Id, userResult.Value.Email, userResult.Value.UserName, roles.Value);

            //Return result 
            return new UserDto()
            {
                Email = loginDto.Email,
                DisplayName = userResult.Value.DisplayName,
                Token = token
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

            var roles = await _identityService.GetUserRolesAsync(user.Email, ct);
            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles.Value);
            return Result<UserDto>.Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = token
            });
            
        }
    }
}
