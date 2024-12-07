using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Serialization;
using WalletAPI.Dtos;
using WalletAPI.Interfaces;
using WalletAPI.Utilities;

namespace WalletAPI.Services
{
    internal sealed class JwtTokenManagerService : IJwtTokenManagerService
    {
        private readonly AppSettings _appSettings;

        public JwtTokenManagerService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<string> GenerateJwtToken(GenerateJwtTokenDto paylod)
        {
            string response = string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = Encoding.ASCII.GetBytes(_appSettings.Jwt.SecretKey);
            var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, paylod.email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim("Email", paylod.email));
            claims.Add(new Claim("FirstName",paylod.firstname));
            claims.Add(new Claim("LastName", paylod.lastname));

            if (paylod.Roles is not null)
            {
                foreach (var role in paylod.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var tokenDescriptor = new JwtSecurityToken(_appSettings.Jwt.Issuer,
                _appSettings.Jwt.Audience, claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(_appSettings.Jwt.TokenValidityInMinute),
                signingCredentials:signinCredentials);
            response =  tokenHandler.WriteToken(tokenDescriptor);

            return response;

        }

    }
}
