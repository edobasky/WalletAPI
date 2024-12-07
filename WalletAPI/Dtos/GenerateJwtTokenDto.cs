namespace WalletAPI.Dtos
{
    public record GenerateJwtTokenDto(string email,string firstname,string lastname,List<string>? Roles = null);
}
