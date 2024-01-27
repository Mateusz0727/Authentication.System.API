using Authentication.System.API.Data;
using Authentication.System.API.Extensions;
using Authentication.System.API.Models;
using Authentication.System.API.Models.Register;
using Authentication.System.API.Models.Settings;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
namespace Authentication.System.API.Services.GoogleAuthService;
public class GoogleAuthService :BaseService
{
    private readonly IOptions<GoogleSettings> _googleSettings;
     private readonly string? _clientId ;
    private readonly string? _clientSecret ;
    private readonly string? _redirectUri ;
    private readonly UserService _userService;

    public GoogleAuthService(IOptions<GoogleSettings> googleSettings, UserService userService, IMapper mapper,BaseContext context) : base(mapper,context )
    {
        _googleSettings = googleSettings;
        _userService = userService;
        _clientId = _googleSettings.Value.clientId;
        _clientSecret=_googleSettings.Value.clientSecret;
        _redirectUri=_googleSettings.Value.redirectUri;
    }
    public async Task<JwtSecurityToken> GetGoogleToken(string request)
    {
        var tokenRequestParameters = new Dictionary<string, string?>
        {
            { "code",request },
            { "client_id", _clientId },
            { "client_secret", _clientSecret },
            { "redirect_uri", _redirectUri },
            { "grant_type", "authorization_code" }
        };
        //sending a POST request to Google to get the token
        using (var httpClient = new HttpClient())
        {
            var tokenEndpoint = "https://oauth2.googleapis.com/token";
            var response = await httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(tokenRequestParameters));
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                  var tokenResponse = JsonConvert.DeserializeObject<GoogleTokenResponse>(responseContent);


                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(tokenResponse.id_token);
                return  jwtToken;       
            }
             throw new Exception($"Error while retrieving Google token. HTTP status code: {response.StatusCode}");
        }
    }
    
    public async Task<User> CreateUser(JwtSecurityToken jwtToken)
    {
        var entity = new RegisterFormModel()
        {
            Email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value,
            SurName = jwtToken.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value,
            GivenName = jwtToken.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value,
            EmailConfirmed = Convert.ToBoolean(jwtToken.Claims.FirstOrDefault(x => x.Type == "confirmed")?.Value)
        };
          
        return await _userService.CreateAsync(entity);

    }
       
    }




public class GoogleTokenResponse
    {
        public string? access_token { get; set; }
        public int expires_in { get; set; }
        public string? token_type { get; set; }
        public string? refresh_token { get; set; }
        public string? id_token { get; set; }
    }