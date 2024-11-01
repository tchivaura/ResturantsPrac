using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Microsoft.OpenApi.Models;

namespace Restaurants.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication();
            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(c =>
            {
                // Adding the Bearer Authentication scheme
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                    //BearerFormat = "JWT",  // Optional, indicates the token format is JWT
                    //In = ParameterLocation.Header,  // Bearer token is passed in the Authorization header
                    //Name = "Authorization",  // Name of the header that will carry the token
                    //Description = "Please insert JWT token into field"
                });

                // Adding security requirement for the Bearer authentication
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new List<string>() // This is an empty list of scopes (used for OAuth2)
        }
                });
            });
            builder.Services.AddScoped<ErrorHandlingMiddle>().AddScoped<ErrorHandlingMiddle>();
            builder.Services.AddEndpointsApiExplorer();

            var configuration = builder.Configuration;

            builder.Host.UseSerilog((context, Configuration) => Configuration.ReadFrom.Configuration(context.Configuration));

        }
    }
}