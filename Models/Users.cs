using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
namespace Authentication.System.API.Models;
public class User
{
    [BsonId]
    public string PublicId { get; set; }
    
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string GivenName { get; set; }

    public string Surname { get; set; }
    
}