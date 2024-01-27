using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Authentication.System.API.Models;
using Authentication.System.API.Models.Settings;
using Authentication.System.API.Services;
using Authentication.System.API.Services.Auth;
using Authentication.System.API.Services.GoogleAuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Authentication.System.API.Controllers.Auth;
    
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
    private readonly GoogleSettings _googleSettings;
    private readonly GoogleAuthService _googleAuthService;
    private readonly AuthService _authService;
    private readonly UserService _userService;

    public GoogleAuthController(GoogleSettings googleSettings,GoogleAuthService googleAuthService,AuthService authService)
        {
        _googleSettings = googleSettings;
        _googleAuthService = googleAuthService;
        _authService= authService;
    }

        [HttpPost]
        [Route("callback")]
        public async Task<IActionResult> GoogleAuthCallback([FromBody] string request)
        {
        JwtSecurityToken jwtToken= await _googleAuthService.GetGoogleToken(request);
            // Read claims from token
            User entity = await _googleAuthService.CreateUser(jwtToken);
                    try
                    {
                         //create token
                        var token = _authService.CreateToken(entity);

                        var options = new CookieOptions()
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        };

            // Set cookie with Token
                        Response.Cookies.Append("jwtToken", JsonConvert.SerializeObject(token), new CookieOptions
                        {
                            Expires = DateTime.UtcNow.AddHours(2)
                        });
                    
                        return Ok();
                    }
                    catch
                    {
                        return BadRequest("Token error");
                    }
            
        }
    }
