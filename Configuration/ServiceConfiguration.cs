using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using be_atoutmajeur.Core;
using be_atoutmajeur.Data;
using be_atoutmajeur.Models.Interfaces;
using be_atoutmajeur.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;

namespace be_atoutmajeur.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        
        // CORS Configuration corrigée
        services.AddCors(options =>
        {
            // Policy pour le développement - PERMET LES COOKIES
            options.AddPolicy("Development", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "http://localhost:5041", "https://localhost:7001")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials(); // ✅ ESSENTIEL pour les cookies
            });
            
            // Policy pour la production
            options.AddPolicy("Production", policy =>
            {
                policy.WithOrigins("https://votredomaine.com") // Remplacez par votre domaine
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials(); // ✅ ESSENTIEL pour les cookies
            });
        });

        // JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // ✅ Permettre au JWT de lire les cookies
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Récupérer le token depuis le cookie si pas dans le header
                        if (string.IsNullOrEmpty(context.Token))
                        {
                            context.Token = context.Request.Cookies["authToken"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        // Configuration des cookies simplifiée
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => false; // ✅ Désactivé pour l'auth
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified; // ✅ Laisse le contrôle au CookieHandler
        });

        // HTTPS Redirection (seulement en production)
        services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            options.HttpsPort = 7001;
        });

        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(60);
        });

        // Services
        services.AddHttpContextAccessor();
        services.AddScoped<CookieHandler>();
        services.AddScoped<ISecurityServices, SecurityCoreApi>();
        services.AddScoped<IAuthService, AuthServices>();

        return services;
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            // ✅ Pas de HTTPS redirection en développement
            app.UseCors("Development"); // ✅ Policy qui permet les cookies
        }
        else
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseCors("Production"); // ✅ Policy production avec credentials
        }
        
        // ✅ Ordre correct du pipeline
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        return app;
    }
}