using Authentication.System.API.Models;
using Authentication.System.API.Services;
using Microsoft.AspNetCore.Mvc;
namespace Authentication.System.API.Controllers;

[Controller]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly MongoDBService _mongoDBService;

    public UsersController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }
    [HttpGet("id")]
    public async Task<User> GetById(long id)
    {
       return await _mongoDBService.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]User user)
    {
       await _mongoDBService.CreateAsync(user);
        return CreatedAtAction(nameof(GetById),new {id = user.Id});
    }

}
