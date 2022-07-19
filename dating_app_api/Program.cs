using dating_app_api.Data;
using dating_app_api.Extensions;
using dating_app_api.Interfaces;
using dating_app_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors();

builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddIdentityService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors(policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
