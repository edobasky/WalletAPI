using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalletAPI.Common;
using WalletAPI.Dtos;
using WalletAPI.Interfaces;

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto accountDto)
        {
            var serviceResponse = new ServiceResponse<dynamic>();

            serviceResponse = await _accountService.CreateAccountAsync(accountDto);

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

    }
}
