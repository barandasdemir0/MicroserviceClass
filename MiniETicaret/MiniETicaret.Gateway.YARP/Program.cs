using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniETicaret.Gateway.YARP.Context;
using MiniETicaret.Gateway.YARP.Dtos;
using MiniETicaret.Gateway.YARP.Models;
using MiniETicaret.Gateway.YARP.Services;
using System.Text;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:SecretKey").Value??"")),
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
});

var app = builder.Build();


app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.MapGet("/", () => "Hello World!");

app.MapPost("/auth/register", async (RegisterDto request, ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    bool isUserNameExisists = await context.Users.AnyAsync(x => x.UserName == request.UserName, cancellationToken);
    if (isUserNameExisists)
    {
        return Results.BadRequest(Result<string>.Failure("Zaten Kullanıcı Mevcut"));
    }

    User user = new()
    {
        UserName = request.UserName,
        Password = request.Password
    };

    await context.AddAsync(user, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(Result<string>.Succeed("Kullanıcı kaydı Başarılı "));
});

app.MapPost("/auth/login", async (LoginDto request, ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    User? userExisists  = await context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);
    if (userExisists is null)
    {
        return Results.BadRequest(Result<string>.Failure("Kullanıcı Mevcut değil"));
    }



    //token üret
    JwtProvider jwtProvider = new(builder.Configuration);
    string token = jwtProvider.CreateToken(userExisists);
    return Results.Ok(Result<string>.Succeed(token));
});


app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

using (var scope = app.Services.CreateScope())
{
    var srv = scope.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
