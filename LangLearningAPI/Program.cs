using Application.Mapping;
using Application.Services.Implementations.Auth.JWT;
using Application.Services.Implementations.Auth;
using Application.Services.Interfaces.IRepository;
using Application.Services.Interfaces.IServices.Auth;
using Application.Services.Interfaces.IServices;
using Application.UnitOfWork;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistance.UnitOfWork;
using System.Text;
using Application.MappingProfile;
using Application.Services.Implementations;
using Persistance.Repositories;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<LanguageLearningDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 26))
            ));

        builder.Services.AddIdentity<Users, IdentityRole>()
            .AddEntityFrameworkStores<LanguageLearningDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
            };
        });

        builder.Services.AddSingleton<IAuthEmailService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var smtpSettings = configuration.GetSection("Email:Smtp");
            return new AuthEmailService(
                smtpSettings["Host"],
                int.Parse(smtpSettings["Port"]),
                smtpSettings["Username"],
                smtpSettings["Password"]
            );
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddLogging();

        builder.Services.AddAutoMapper(typeof(MappingAuthProfile), typeof(MappingUserProfile));


        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            await SeedData(serviceProvider);
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static async Task SeedData(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        await SeedRoles(roleManager, logger);
        await SeedAdminUser(userManager, logger);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager, ILogger logger)
    {
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    logger.LogInformation($"Role '{roleName}' created successfully.");
                }
                else
                {
                    logger.LogError($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

    private static async Task SeedAdminUser(UserManager<Users> userManager, ILogger logger)
    {
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new Users
            {
                UserName = "admin",
                Email = "admin@example.com"
            };
            var createUserResult = await userManager.CreateAsync(adminUser, "Admin123!");
            if (createUserResult.Succeeded)
            {
                logger.LogInformation("Admin user created successfully.");
                var addRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                if (addRoleResult.Succeeded)
                {
                    logger.LogInformation("Admin role assigned to the user.");
                }
                else
                {
                    logger.LogError($"Failed to assign Admin role: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                logger.LogError($"Failed to create admin user: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
            }
        }
    }
}