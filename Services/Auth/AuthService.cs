using Authentication.System.API.Configuration;
using Authentication.System.API.Helpers;
using Authentication.System.API.Models;
using Authentication.System.API.Models.Login;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
namespace Authentication.System.API.Services.Auth;
public class AuthService
{
    private JWTConfig _config;
    protected IPasswordHasher<User> Hasher { get; }

    public AuthService(JWTConfig config,IPasswordHasher<User> hasher)
    {
        _config = config;
          Hasher= hasher;
    }
    public bool Login(LoginFormModel login, User user)
    {
        var result = Hasher.VerifyHashedPassword(user, user.Password, login.Password);
        if (result == PasswordVerificationResult.Success)
        {
            return true;
        }

        return false;
    }
    #region CreateToken()
    public UserTokens CreateToken(User user)
        {
         var Token = new UserTokens();

            Token = JWTHelper.GenTokenKey(new UserTokens()
            {
                EmailId = user.Email,
                GuidId = Guid.NewGuid(),
                UserName = user.UserName,
                Id = user.Id,

            }, _config, user);
         
            return Token;
        }


        #endregion

}
