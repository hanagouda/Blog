using BlogAPI.Data;
using BlogAPI.Middleware;
using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (builder.Environment.EnvironmentName != "Testing")
        {
            builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        }
        builder.Services.AddScoped<PostService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddScoped<TagService>();
        builder.Services.AddScoped<CommentService>();
        builder.Services.AddScoped<LikeService>();

        var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
        builder.Services.AddSingleton(jwtOptions);
        builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Signingkey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1) // means if a token expires at 12:00:00, it will still be considered valid until 12:01:00.
            };
        });

        const string CorsPolicy = "DefaultCors";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policy =>
            {
                policy.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
        builder.Services.AddAutoMapper(typeof(Program));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseCors(CorsPolicy);
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

// 👇 required for Integration Tests.
public partial class Program { }
