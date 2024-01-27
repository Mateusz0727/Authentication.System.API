using Authentication.System.API.Models;
using Authentication.System.API.Services;
using Microsoft.AspNetCore.Mvc;
namespace Authentication.System.API.Controllers;

[Controller]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }
    [HttpGet("id")]
    public async Task<User> GetById(long id)
    {
       return await _userService.GetByIdAsync(id);
    }
    [HttpGet]
    public async Task<List<User>> Get(CancellationToken cancellationToken)
    {
        return await _userService.GetAsync(cancellationToken);
    }
   

}
