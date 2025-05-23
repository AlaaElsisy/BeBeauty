
using System.Text;
using BeBeauty.Mapping;
using BeBeauty.Models;
using BeBeauty.Models.identity;
using BeBeauty.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;

namespace BeBeauty
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region AddDbContext AddIdentity
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication();
            #endregion
            #region jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateLifetime = true,
                   ValidateAudience = false,
                   ValidateIssuer = false,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.ASCII.GetBytes(builder.Configuration["jwt:key"])),
               };
           });

            #endregion

            #region add cors
            builder.Services.AddEndpointsApiExplorer();
            string txt = "";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            #endregion

            #region autoMapper
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            #endregion


            builder.Services.AddScoped <GenericRepo<Category>>();
            builder.Services.AddScoped<ProductRepo>();
            builder.Services.AddScoped<OrderRepo>();
            builder.Services.AddScoped<CartRepo>();
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                  
                app.MapOpenApi();
               
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
          
            app.UseAuthentication();  
            app.UseAuthorization();
            app.UseCors(txt);
            app.MapControllers();

            app.Run();
        }
    }
}
