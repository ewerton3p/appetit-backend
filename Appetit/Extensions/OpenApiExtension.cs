using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Appetit.API.Extensions
{
    public static class OpenApiExtension
    {
        public static IServiceCollection AddOpenApiWithAuth(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Informe o token JWT no campo abaixo."
                    };
                    document.Security ??= [];
                    document.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer")] = []
                    });
                    return Task.CompletedTask;
                });
            });

            return services;
        }

        public static IApplicationBuilder UseOpenApiWithScalar(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Authentication = new ScalarAuthenticationOptions
                    {
                        PreferredSecuritySchemes = ["Bearer"]
                    };
                });
            }

            return app;
        }
    }
}
