﻿using WalletAPI.Common;
using WalletAPI.Dtos;

namespace WalletAPI.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<dynamic>> CreateAccountAsync(CreateAccountDto userCreate);
    }
}
