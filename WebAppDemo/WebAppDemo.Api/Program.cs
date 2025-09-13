using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using System.Text;
using WebApp.Application.mappers;
using WebApp.Core.Entities;
using WebApp.Infrastructure.persistence;
using WebApp.Infrastructure.Persistence;
using WebAppDemo.Api.Middelwares;

namespace WebAppDemo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.AddMemoryCache();

            var key = builder.Configuration["Jwt:Key"];
            var keyBytes = Encoding.UTF8.GetBytes(key);

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
                   .AddEntityFrameworkStores<AppDbContext>()
                   .AddDefaultTokenProviders();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.SaveToken = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidIssuer = builder.Configuration["Jwt:Issuer"],
                           ValidAudience = builder.Configuration["Jwt:Audience"],
                           ClockSkew = TimeSpan.Zero,

                       };
                   });

            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Information()
                 .WriteTo.MSSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                     sinkOptions: new MSSqlServerSinkOptions
                     {
                         TableName = "Logs",
                         AutoCreateSqlTable = true  
                     })
                 .CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddSerilogUi(options =>
            {
                options.UseSqlServer(opts =>
                    opts.WithConnectionString(builder.Configuration.GetConnectionString("DefaultConnection")!)
                        .WithTable("Logs"));
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseSerilogUi(options =>
                {
                    options.WithRoutePrefix("serilog-ui");
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
