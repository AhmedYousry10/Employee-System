using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Employee_Management_System.Models;
using Employee_Management_System.MappingProfile;
using Employee_Management_System.Service;
using Employee_Management_System;
using Employee_Management_System.IService;
using Hangfire;
using Hangfire.SqlServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure DbContext
        builder.Services.AddDbContext<appContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configure Identity with custom User and Role types
        builder.Services.AddIdentity<Employee, Role>()
            .AddEntityFrameworkStores<appContext>()
            .AddDefaultTokenProviders();

        // Configure AutoMapper
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

        // Register Services
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<JwtTokenService>();
        builder.Services.AddScoped<IRoleService,  RoleService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<DeactiveEmployeeService>();

        // Configure Hangfire for SQL Server
        builder.Services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                  {
                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                      QueuePollInterval = TimeSpan.Zero,
                      UseRecommendedIsolationLevel = true,
                      DisableGlobalLocks = true
                  }));

        builder.Services.AddHangfireServer();

        // Configure JWT
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

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
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        // Add Controllers
        builder.Services.AddControllers();

        // Add CORS policy
        builder.Services.AddCors(Services =>
        {
            Services.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.EnableAnnotations();
        });

        var app = builder.Build();

        // Ensure default roles exist
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleService = services.GetRequiredService<IRoleService>();
            await roleService.EnsureDefaultRolesAsync();

            var authService = services.GetRequiredService<IAccountService>();
            await authService.EnsureDefaultAdminAsync();
        }

        // Schedule the job to run every hour
        var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<DeactiveEmployeeService>("Deactive-Inactive-Employees",
            service => service.DeactivateEmployeesAsync(),
            Cron.Hourly);

        // Use Hangfire for background tasks
        app.UseHangfireDashboard();
        app.UseHangfireServer();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
