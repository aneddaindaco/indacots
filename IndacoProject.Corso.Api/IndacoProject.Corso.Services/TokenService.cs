using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Services
{
    public class TokenService : ITokenService
    {
        protected readonly JWTOptions _jwtOptions;
        protected readonly ICipherService _cipherService;
        public TokenService(IOptions<JWTOptions> jwtOptions, ICipherService cipherService)
        {
            _jwtOptions = jwtOptions.Value;
            _cipherService = cipherService;
        }

        public string GenerateAccessToken(string username)
        {
            if (username == null)
            {
                return null;
            }
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtOptions.Issuer,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(string username)
        {
            if (username == null)
            {
                return null;
            }
            var data = $"{username};{DateTime.UtcNow.AddDays(30).Ticks}";
            return _cipherService.EncryptString(data, _jwtOptions.Secret);
        }

        public string VerifyRefreshToken(string token)
        {
            if (token == null)
            {
                return null;
            }
            try
            {
                var u = _cipherService.DecryptString(token, _jwtOptions.Secret).Split(";");

                if (u.Length != 2)
                {
                    throw new Exception("Invalid token");
                }
                if (new DateTime(long.Parse(u[1])) <= DateTime.UtcNow)
                {
                    throw new Exception("Expired token");
                }
                return u[0];
            }
            catch (Exception)
            {
                throw new Exception("Invalid token");
            }
        }
    }
}
