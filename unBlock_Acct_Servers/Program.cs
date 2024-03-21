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
    try{
        JwtSecurityToken DecodedToken = (JwtSecurityToken)(new JwtSecurityTokenHandler().ReadToken(Token));
        var Payload = DecodedToken.Payload;
        var TokenHeader = DecodedToken.Header;
        string Email = Payload["preferred_username"].ToString();
        context.Request.Headers["email"] = Email;

        /////////////////////////////////// VERIFY TOKEN //////////////////////////////////////
        var IssuedAt = DecodedToken.IssuedAt;
        var Now = DateTime.Now;
        var HourDiff = IssuedAt.Subtract(Now).Hours;
        if (HourDiff > 24)
        {
            Valid = false;
        }
        else
        {
            /////////////////////////// SEARCH FOR PUBLIC KEY ////////////////////////////
            var TokenKid = TokenHeader["kid"]; //key id to search for Microsoft's public key
            var response = await new HttpClient().GetAsync("https://login.microsoftonline.com/common/discovery/keys?appid=b05a8050-78a7-4a57-bd62-fe28df281cfd");
            var strContent = await response.Content.ReadAsStringAsync();
            JObject jsonContent = JObject.Parse(strContent);
            JArray PublicKeys = (JArray)jsonContent["keys"];
            JObject[] PublicKeysFilter = PublicKeys.Where(o => ((string)o["kid"]).Equals(TokenKid)).Select(o => (JObject)o).ToArray();
            var N = PublicKeysFilter[0]["n"].ToString();
            var E = PublicKeysFilter[0]["e"].ToString();

            ///////////////////// VALIDATE TOKEN WITH PUBLIC KEY ////////////////////////
            //https://stackoverflow.com/questions/34403823/verifying-jwt-signed-with-the-rs256-algorithm-using-public-key-in-c-sharp
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(
              new RSAParameters()
              {
                  Modulus = FromBase64Url(N),
                  Exponent = FromBase64Url(E)
              }
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };
            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(Token, validationParameters, out validatedToken);
        }
    }
    catch (Exception)
    {
        Valid = false;
    }

    if (!Valid)
    {
        // Reject invalid token
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("An error occurred within the API gateway middleware. Please try again later.");
    }
    else
    {
        // Call the next delegate/middleware in the pipeline.
        await next(context);
    }
});

app.Run();
