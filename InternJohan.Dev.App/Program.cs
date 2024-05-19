using InternJohan.Dev.API.Services;
using InternJohan.Dev.Infrastructure;
using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using InternJohan.Dev.App;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; 
using System.Text;
using InternJohan.Dev.Infrastructure.Services; 

var builder = WebApplication.CreateBuilder(args);

// Konfigurera anslutningsstr�ngar och andra inst�llningar
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));

// L�gg till Entity Framework Core-tj�nsten och anv�nd din ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Anv�nd den anslutningsstr�ng du definierade i appsettings.json
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// L�gg till dina andra tj�nster (repositories, services, etc.)
builder.Services.AddTransient<SportEventRepository>();
builder.Services.AddTransient<SportEventService>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RoleRepository>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddTransient<PostRepository>();
builder.Services.AddTransient<PostReplyRepository>();
builder.Services.AddTransient<ReplyRepository>();
builder.Services.AddTransient<ReplyService>();
builder.Services.AddTransient<PostReplyService>();
builder.Services.AddTransient<PostService>();
builder.Services.AddTransient<AttendeeService>();
builder.Services.AddTransient<AttendeeRepository>();


// L�gg till controllers
builder.Services.AddControllers();
builder.Services.AddScoped<PasswordService>();

// L�gg till Swagger/OpenAPI om du anv�nder det
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// L�gg till CORS-policy om du har en
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsRule", builder =>
    {
        builder.WithOrigins("http://localhost:8080")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// L�gg till `IHttpContextAccessor`-tj�nsten
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// L�gg till autentisering och auktoriseringstj�nster
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // L�gg till JWT-autentiseringstj�nster
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Anv�nd din JWT-utf�rdare (issuer)
            ValidAudience = builder.Configuration["Jwt:Audience"], // Anv�nd din JWT-publik (audience)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Anv�nd din JWT-signeringsnyckel
        };
    });



var app = builder.Build();

// Anv�nd CORS-policy om du har definierat en
app.UseCors("CorsRule");

// Om applikationen �r i utvecklingsl�ge, anv�nd Swagger och SwaggerUI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Anv�nd HTTPS om det beh�vs
app.UseHttpsRedirection();

// Anv�nd autentisering och auktorisering
app.UseAuthentication(); // L�gg till autentiseringstj�nster
app.UseAuthorization();

// Mappa controllers
app.MapControllers();

// Konfigurera SPA om du har en client app
app.MapWhen(x => !x.Request.Path.StartsWithSegments("/api"), x =>
{
    app.UseSpa(spa =>
    {
        if (!builder.Environment.IsDevelopment())
            spa.Options.SourcePath = "wwwroot/clientapp";
        else
            spa.Options.SourcePath = @"C:\Data\Repos\KEYnet\InternJohan.Dev.App\wwwroot\clientapp";
    });
});

// K�r applikationen
app.Run();