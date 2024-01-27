using Authentication.System.API.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text.Json.Serialization;
namespace Authentication.System.API.Models;
[Table("Users")]
public class User:BaseModel
{
    [BsonId]
    
    public string? PublicId { get; set; }
    
    public int Id { get; set; }

    public string? UserName { get; set; }

    
    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? GivenName { get; set; }

    public string? Surname { get; set; }
    public string? Password { get; set; }

    public DateTime DateCreatedUtc { get; set; }
    public DateTime DateModifiedUtc { get; set; }

}