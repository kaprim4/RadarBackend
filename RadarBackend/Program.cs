using Domain.Helper;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RadarBackend.Helper;
using Services;
using Services.Auth;
using Services.DataService;
using Services.DeviceService;
using Services.ImageService;
using Services.IMageService;
using Services.UserService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register Infrastructure services (Database context, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Add JWT Settings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.Configure<ResourceSettings>(builder.Configuration.GetSection("ResourceSettings"));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
    });
});

// Register application services (business logic layer)
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ImageHelper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add controllers (API endpoints)
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT Bearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 524288000; // 500 MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    // Enable Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RadarBackend API v1"));
}
//app.UseHttpsRedirection();
app.UseRouting();


app.UseCors("AllowSpecificOrigin");


// Enable authentication middleware
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

// Map controllers (API endpoints)
app.MapControllers();

app.Run();
