using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Identity;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IdentityUserResult>.Fail(Error.NotFound("User.NotFound", $"User with email {email} is not found"));
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

        public async Task<Result<AddressDto>> GetUserAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user?.Address is null) return Result<AddressDto>.Fail(Error.NotFound("Address Not Found!", $"User With Email {email}, Does Not Have A Registered Address!"));
            else
            {
                var address = user.Address;
                return Result<AddressDto>.Ok((new AddressDto()
                {
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    City = address.City,
                    Street = address.Street,
                    Country = address.Country
                }));

            }
        }

        public async Task<Result<AddressDto>> UpSertUserAddresAsync(string email, AddressDto addressDto, CancellationToken ct = default)
        {
            var user = _userManager.Users.Include(x => x.Address).FirstOrDefault(x => x.Email == email);
            if (user!.Address is null)
            {
                //Insert
                user.Address = new Address()
                {
                    FirstName = addressDto.FirstName,
                    LastName = addressDto.LastName,
                    City = addressDto.City,
                    Country = addressDto.Country,
                    Street = addressDto.Street

                };
            }
            else
            {
                //Update
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                user.Address.Street = addressDto.Street;
            }

            var res = await _userManager.UpdateAsync(user);
            return res.Succeeded ? Result<AddressDto>.Ok(addressDto) : Result<AddressDto>.Fail(Error.Failure("Failed To Insert / Update Address",string.Join(';',res.Errors.Select(x => x.Description))));
        }
    }
}
