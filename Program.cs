using FlyHigh.Data;
using FlyHigh.Models;
using FlyHigh.Models.MatchModels;
using FlyHigh.Models.TeamModels;
using FlyHigh.Repositories;
using FlyHigh.Services;
using FlyHigh.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FlyHigh
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ===============================
            // 1. DATABASE & ENTITY FRAMEWORK
            // ===============================
            builder.Services.AddDbContext<VolleyballDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ===============================
            // 2. ASP.NET IDENTITY
            // ===============================
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // User settings
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<VolleyballDbContext>()
            .AddDefaultTokenProviders();

            // ===============================
            // 3. JWT AUTHENTICATION
            // ===============================
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // ===============================
            // 4. REPOSITORY PATTERN
            // ===============================
            builder.Services.AddScoped<IMatchRepository, MatchRepository>();

            // ===============================
            // 5. SERVICE LAYER
            // ===============================
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IPollService, PollService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ITrainingService, TrainingService>();

            // ===============================
            // 6. CORS POLICY
            // ===============================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // ===============================
            // 7. API CONTROLLERS & SWAGGER
            // ===============================
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FlyHigh Volleyball System API",
                    Version = "v1",
                    Description = "Kompletní API pro správu volejbalových týmů, zápasů, tréninků a anket podle českého volejbalu"
                });

                // JWT Bearer token support ve Swaggeru
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // ===============================
            // 8. BUILD APPLICATION
            // ===============================
            var app = builder.Build();

            // ===============================
            // 9. MIDDLEWARE PIPELINE
            // ===============================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlyHigh API V1");
                    c.RoutePrefix = string.Empty; // Swagger UI na root URL
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            // Authentication PŘED Authorization!
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // ===============================
            // 10. DATABASE SEEDING
            // ===============================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<VolleyballDbContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // Zajistit vytvoření/update databáze
                    await context.Database.MigrateAsync();

                    // Seed základní role
                    await SeedRolesAsync(roleManager);

                    // Seed admin uživatele
                    await SeedAdminUserAsync(userManager, context, builder.Configuration);

                    Console.WriteLine("✅ Databáze a seedování dokončeno úspěšně!");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "❌ Chyba při inicializaci databáze a seedování");
                    throw; // Zastavit aplikaci pokud se seedování nepovede
                }
            }

            Console.WriteLine($"🚀 FlyHigh API běží na: {string.Join(", ", app.Urls)}");
            await app.RunAsync();
        }

        // ===============================
        // SEED METHODS
        // ===============================
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Coach", "Player", "Manager" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"✅ Role '{roleName}' byla vytvořena");
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<User> userManager, VolleyballDbContext context, IConfiguration configuration)
        {
            var adminEmail = configuration["Seed:AdminEmail"] ?? "admin@flyhigh.com";
            var adminPassword = configuration["Seed:AdminPassword"] ?? "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    CreatedAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"✅ Admin uživatel vytvořen: {adminEmail}");

                    // Vytvoř ukázkový tým pro admina
                    var sampleTeam = new Team
                    {
                        Name = "FlyHigh Demo Team",
                        Description = "Ukázkový tým pro testování aplikace",
                        City = "Praha",
                        ContactEmail = adminEmail,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    context.Teams.Add(sampleTeam);
                    await context.SaveChangesAsync();

                    // Přidej admina do týmu jako Coach
                    var membership = new TeamMember
                    {
                        UserId = adminUser.Id,
                        TeamId = sampleTeam.Id,
                        Role = TeamRole.Coach,
                        IsActive = true,
                        JoinedAt = DateTime.Now
                    };

                    context.TeamMembers.Add(membership);
                    await context.SaveChangesAsync();

                    Console.WriteLine($"✅ Ukázkový tým '{sampleTeam.Name}' byl vytvořen");
                }
                else
                {
                    Console.WriteLine($"❌ Chyba při vytváření admin uživatele: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine("ℹ️ Admin uživatel již existuje");
            }
        }
    }
}
