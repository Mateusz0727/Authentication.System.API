using Authentication.System.API.Configuration;
using Authentication.System.API.Services.Auth;
using Authentication.System.API.Services;
using Microsoft.AspNetCore.Mvc;
using Authentication.System.API.Models.Login;
using Authentication.System.API.Models.Register;

namespace Authentication.System.API.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly JWTConfig _config;
        public AuthController(AuthService authService, JWTConfig config, UserService userService)
        {
            _userService = userService;
            _authService = authService;
            _config = config;
        }
        #region Login()
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserTokens>> Login([FromBody] LoginFormModel model)
        {
            if (ModelState.IsValid)
            {
                var Token = new UserTokens();
                var entity =await _userService.GetByEmailAsync(model.Email);
                if (entity != null)
                {
                    var result = _authService.Login(model, entity);
                    if (result)
                    {
                        return Ok(_authService.CreateToken(entity));
                    }
                    else
                    {
                        return BadRequest($"wrong password");
                    }
                }
            }

            return StatusCode(401, "[[[Nazwa użytkownika lub hasło są nieprawidłowe.]]]");
        }
        #endregion
        #region Register()
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<object> Register([FromBody] RegisterFormModel model)
        {

            var entity = _userService.CreateAsync(model);

            return Created($"~api/users/{entity.Id}", entity);

        }
        #endregion
    }
}
