using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{

    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        //Login
        [HttpPost("Login")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct = default)
            => ToActionResult(await _authenticationService.LoginAsync(loginDto, ct));


        //Register
        [HttpPost("Register")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct = default)
            => ToActionResult(await _authenticationService.RegisterAsync(registerDto, ct));

        //EmailExists
        [HttpGet("EmailExists/{email}")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email, CancellationToken ct = default)
        {
            var result = await _authenticationService.CheckEmailExistsAsync(email, ct);
            return ToActionResult(result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct = default)
            => ToActionResult(await _authenticationService.GetCurrentUserAsync(GetEmailFromToken(), ct));



        [Authorize]
        [HttpGet("UserAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress(CancellationToken ct = default)
            => ToActionResult(await _authenticationService.GetUserAddressAsync(GetEmailFromToken(), ct));

        [Authorize]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto, CancellationToken ct = default)
          => ToActionResult(await _authenticationService.UpSertUserAddressAsync(GetEmailFromToken(), addressDto, ct));

    }
}
