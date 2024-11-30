namespace WalletAPI.Dtos
{
    public record CreateAccountDto(string Email,
        string FirstName,
        string LastName,
        string Password,
        string Address,
        string PhoneNumber);
}
