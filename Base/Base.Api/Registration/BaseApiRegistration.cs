using Base.Application;
using Base.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using System.Reflection.Metadata;

//using Microsoft.OpenApi.Models;
using System.Text;

namespace Base.Api.Registration
{
    public static class BaseApiRegistration
    {
        public static void RegisterBaseApi(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var services = builder.Services;

            builder.RegisterBaseApplication(configuration);

            services.AddControllers();
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();

            #region Swagger
            services.ConfigureSwagger();
            #endregion 

            services.AddCors(options =>
            {
                options.AddPolicy("policy", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();

                });
                options.AddPolicy("chat", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)  // Temporary for testing
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
                    //builder.WithOrigins("http://localhost:5173")
                    //.AllowAnyHeader()
                    //.AllowAnyMethod()
                    //.AllowCredentials();

                    //builder.WithOrigins("http://62.60.213.13")
                    //.AllowAnyHeader()
                    //.AllowAnyMethod()
                    //.AllowCredentials();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = false,
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.RegisterBaseInfra(configuration);
        }

        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddOpenApi();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement((document) => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });
        } 
    }
}
