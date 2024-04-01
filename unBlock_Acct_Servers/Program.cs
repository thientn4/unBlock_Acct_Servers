using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//////// MIDDLEWARE AS API GATEWAY
static byte[] FromBase64Url(string base64Url)
{
    string padded = base64Url.Length % 4 == 0
    ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
    string base64 = padded.Replace("_", "/")
                          .Replace("-", "+");
    return Convert.FromBase64String(base64);
}
app.Use(async (context, next) =>
{
    bool Valid = true;

    var Request = context.Request;
    var Header = Request.Headers;
    var AuthType = Header["token"].ToString()[0];
    var Token = Header["token"].ToString().Substring(1);
    try
    {
        JwtSecurityToken DecodedToken = (JwtSecurityToken)(new JwtSecurityTokenHandler().ReadToken(Token));
        var Payload = DecodedToken.Payload;
        string Email = Payload["preferred_username"].ToString().ToLower();
        context.Request.Headers["email"] = Email;
        string AppId = Payload["aud"].ToString();
        var IssuedAt = DecodedToken.IssuedAt;
        Valid = (
            DateTime.Now.Subtract(IssuedAt).TotalHours <= 24 &&
            AppId == "b05a8050-78a7-4a57-bd62-fe28df281cfd"
        );
    }
    catch (Exception)
    {
        Valid = false;
    }

    if (!Valid)
    {
        // Reject invalid token
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("invalid token");
    }
    else
    {
        // Call the next delegate/middleware in the pipeline.
        await next(context);
    }
});

app.Run();
