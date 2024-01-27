using Authentication.System.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using Authentication.System.API.Models.Settings;
using Authentication.System.API.Data;
using AutoMapper;
using Authentication.System.API.Models.Register;
using Microsoft.AspNetCore.Identity;
using System.Threading;

namespace Authentication.System.API.Services;

public class UserService :BaseService
{
    private readonly IMongoCollection<User> _usersCollection;
    protected IPasswordHasher<User> Hasher { get; }
    public UserService(IMapper mapper,IPasswordHasher<User> hasher, BaseContext context):base(mapper,context)
    {
        _usersCollection = Context.Users;
        Hasher= hasher;
    }
    public  async Task<User> GetByIdAsync(long id)
    {
        return await _usersCollection.Find(x=>x.Id ==id).FirstOrDefaultAsync();
        
    }
    public async Task<List<User>> GetAsync(CancellationToken cancellationToken)
    {
        return await _usersCollection.Find(Builders<User>.Filter.Empty).ToListAsync(cancellationToken);

    }
    public  async Task<User> GetByEmailAsync(string email)
    {
       User entity= await  _usersCollection.FindAsync(x=>x.Email ==email).Result.FirstOrDefaultAsync();
        return entity;


    }
   public  async Task<User> CreateAsync(RegisterFormModel user)
   {
        var entity = Mapper.Map<User>(user);
        try
        {
            
            SetPassword(entity, user.Password);
            SetEntity(entity);
         await   Context.Add<User>(entity, CancellationToken.None);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.Message.Contains("IX_Email"))
                    throw new Exception("Podany adres email jest zajêty");
                if (ex.InnerException.Message.Contains("IX_UserName"))
                    throw new Exception("Podany adres email jest zajêty");
            }
        }
        return entity;
        
   }
    #region SetPassword()
    public bool SetPassword(User user, string password)
    {
        if (user != null)
        {
            user.Password = Hasher.HashPassword(user, password);
            return true;
        }

        return false;
    }
    #endregion
    public async Task SetEntity(User user)
    {
        if (user != null)
        {
            user.PublicId = Guid.NewGuid().ToString();
            user.UserName = user.Email;
            user.DateCreatedUtc = DateTime.Now;
            user.DateModifiedUtc = DateTime.Now;

        }

    }
}
