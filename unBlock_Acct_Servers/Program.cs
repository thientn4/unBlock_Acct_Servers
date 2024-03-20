using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

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
        var Email = Payload["preferred_username"];
        var IssuedAt = DecodedToken.IssuedAt;
    }
    catch (Exception)
    {
        Valid = false;
    }

    if (Valid)
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
