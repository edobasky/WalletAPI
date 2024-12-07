using WalletAPI.Dtos;

namespace WalletAPI.Interfaces
{
    public interface IJwtTokenManagerService
    {
        Task<string> GenerateJwtToken(GenerateJwtTokenDto paylod);
    }
}
