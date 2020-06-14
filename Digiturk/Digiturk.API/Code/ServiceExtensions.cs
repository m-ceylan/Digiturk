using Digiturk.Repository.Application;
using Digiturk.Repository.Definition;
using Digiturk.Repository.ProjectUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digiturk.API.Code
{
    public static class ServiceExtensions
    {
        public static void ConfigureJWTAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(jwtBearerOptions =>
              {
                  jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateActor = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "https://www.digiturk.com.tr",
                      ValidAudience = "https://www.digiturk.com.tr",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("digiturk2020!!1!1!!1trm"))
                  };
              }
          );
        }

        public static void ConfigureRepositories(this IServiceCollection services, string connectionString,string dataBase)
        {

            services.AddSingleton(x => new ArticleRepository(connectionString, dataBase, "article"));
            services.AddSingleton(x => new UserRepository(connectionString, dataBase, "user"));
            services.AddSingleton(x => new CategoryRepository(connectionString, dataBase, "category"));

        }

      
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Digiturk API", Version = "1.0" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

    }
}
