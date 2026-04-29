using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace AuctionHub.Infrastructure.Extensions
{
    public static class ConfigureSwaggerSecureDefinitionExtensions
    {
        public static IServiceCollection AddSwaggerSecureDefinition(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
             {
                 options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                 {
                     Name = "Authorization",
                     Type = SecuritySchemeType.Http,
                     Scheme = "bearer",
                     BearerFormat = "JWT",
                     In = ParameterLocation.Header,
                     Description = "Digite apenas o token"
                 });

                 options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                 {
                     {
                         new OpenApiSecuritySchemeReference("Bearer", doc),
                         []
                     }
                 });
             });

            return services;
        }
    }
}
