using Microsoft.AspNetCore.Identity;
using System.Net;
using WalletAPI.Common;
using WalletAPI.Dtos;
using WalletAPI.Entities;
using WalletAPI.Interfaces;

namespace WalletAPI.Services
{
    public sealed class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ServiceResponse<dynamic>> CreateAccountAsync(CreateAccountDto userCreate)
        {
            var serviceResponse = new ServiceResponse<dynamic>();

            try
            {
                var isUserExist = await _userManager.FindByEmailAsync(userCreate.Email);

                if (isUserExist is not null)
                {
                    serviceResponse.Data = new { };
                    serviceResponse.Message = "User already exist";
                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;

                    return serviceResponse;
                }

                var newUser = new User
                {
                    FirstName = userCreate.FirstName,
                    LastName = userCreate.LastName, 
                    Email = userCreate.Email,
                    Address = userCreate.Address,
                    PhoneNumber = userCreate.PhoneNumber,
                    UpdateAt = DateTime.UtcNow,
                    UserName = userCreate.Email,

                    
                };

                var response = await _userManager.CreateAsync(newUser,userCreate.Password);

                if (!response.Succeeded)
                {
                    var errorMsg = string.Empty;
                    foreach (var error in response.Errors)
                    {
                        errorMsg = error.Description;
                    }

                    serviceResponse.Data = new {};
                    serviceResponse.Message = errorMsg;
                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    return serviceResponse;
                }

                serviceResponse.Data = userCreate;
                serviceResponse.StatusCode = (int)HttpStatusCode.OK;
                serviceResponse.Message = "Customer Registration Successful";
                 serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Data = userCreate;
                serviceResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                serviceResponse.Message = "We are unable to complete your request at this time, Please try again";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<dynamic>> LoginAsync(LoginDto loginDto)
        {
           var serviceResponse = new ServiceResponse<dynamic>();

            try
            {
                var isUserExist = await _userManager.FindByEmailAsync(loginDto.Email);

                if (isUserExist is null)
                {
                    serviceResponse.Message = "Invalid Email or pasword";
                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    return serviceResponse;
                }

                var signInResponse = await _signInManager.PasswordSignInAsync(loginDto.Email,loginDto.password,true,true);

                if (signInResponse.IsLockedOut)
                {
                    await _userManager.SetLockoutEndDateAsync(isUserExist,DateTimeOffset.Now.AddMinutes(15));

                    serviceResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                    serviceResponse.Message = "You have been temporarily locked out";
                    serviceResponse.Success = true;
                    return serviceResponse;
                }

                if (signInResponse.IsNotAllowed)
                {
                    serviceResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                    serviceResponse.Message = "You are not allowed to access this application at this time";
                    serviceResponse.Success = true;
                    return serviceResponse;
                }

                if (!signInResponse.Succeeded)
                {
                    serviceResponse.Message = "Invalid Email or pasword";
                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    return serviceResponse;
                }

                serviceResponse.Message = "Login Successful";
                serviceResponse.StatusCode= (int)HttpStatusCode.OK;
                serviceResponse.Success = true;
            }
            catch (Exception)
            {

                throw;
            }

            return serviceResponse;
        }
    }
}
