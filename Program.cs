using Authentication.System.API.Models;
using Authentication.System.API.Models.Settings;
using Authentication.System.API.Services;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using MongoDB.Driver.Core.Configuration;
using Authentication.System.API.Data;
using ParkBee.MongoDb.DependencyInjection;
using Authentication.System.API.Configuration;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Diagnostics;
using MongoDB.Driver;
using Authentication.System.API.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(options=>
{
 options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})

.AddCookie()
.AddGoogle(options =>

{
    options.ClientId = "19271665004-83jjellktus6s3o000gc90g685lhrp91.apps.googleusercontent.com";
    options.ClientSecret = "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";

});
var mongoDBSetting = new MongoDBSetting();
builder.Configuration.Bind("MongoDB", mongoDBSetting);
var googleSettings = new GoogleSettings();
var bindJwtSettings = new JWTConfig();
builder.Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
builder.Services.AddSingleton(bindJwtSettings);
builder.Services.AddSingleton(googleSettings);
builder.Services.AddSingleton(mongoDBSetting);
builder.Services.AddSingleton<BaseContext>(provider =>
        new BaseContext(mongoDBSetting.ConnectionURI, mongoDBSetting.DatabaseName));
builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleAuth"));
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JsonWebTokenKeys"));

builder.Services.RegisterServices();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
#region AutoMapper
var mappingCongig = new MapperConfiguration(mc =>
     mc.AddProfile(
 new AutoMapperInitializator()
 )
);
IMapper mapper = mappingCongig.CreateMapper();
builder.Services.AddSingleton(mapper);


#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:3001")
        .AllowAnyMethod()
        .AllowAnyHeader();
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
