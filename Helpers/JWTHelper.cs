using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


using System.Text;
using Authentication.System.API.Configuration;
using Authentication.System.API.Models;


namespace Authentication.System.API.Helpers
{
    public static class JWTHelper
    {

        public static List<Claim> GetClaims(this UserTokens userAccounts, long Id,User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("Id", userAccounts.Id.ToString()),
                new Claim(ClaimTypes.Name, userAccounts.UserName),
                new Claim(ClaimTypes.Email, userAccounts.EmailId),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };
           
            return claims;
        }
        public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, out long Id, User user)
        {
            Id = new Random().Next();

            return userAccounts.GetClaims(Id, user);
        }
        public static UserTokens GenTokenKey(UserTokens model, JWTConfig jwtSettings, User user)
        {
            try
            {
                var UserToken = new UserTokens();
                if (model == null) throw new ArgumentException(nameof(model));
                var key = Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                long Id = 0;
                DateTime expireTime = DateTime.UtcNow.AddDays(1);
                UserToken.Validaty = expireTime.TimeOfDay;
                var JWToken = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: model.GetClaims(out Id, user),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(expireTime).DateTime,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
                UserToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                UserToken.UserName = model.UserName;
                UserToken.Id = model.Id;
                UserToken.GuidId = Guid.NewGuid();
                return UserToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
