using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public static class CorsStartup
    {
        public static void AddCorsCustomizado(this IServiceCollection servicos, IConfiguration configuracao)
        {
            servicos.AddCors(o =>
            {
                o.AddPolicy(name: "PermitirTodos", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });

                //o.AddPolicy(name: "PermitirAmbiente", builder =>
                //{
                //    builder.WithOrigins(configuracao.GetSection("Dominios:Supermix:UrlBase").Value,
                //        "file://")
                //        .SetIsOriginAllowedToAllowWildcardSubdomains()
                //        .AllowAnyMethod()
                //        .AllowAnyHeader()
                //        .AllowCredentials();
                //});
            });
        }

        public static void UseCorsCustomizado(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.Equals("loc", System.StringComparison.CurrentCultureIgnoreCase) || 
                env.EnvironmentName.Equals("dev", System.StringComparison.CurrentCultureIgnoreCase) ||
                env.EnvironmentName.Equals("dev-aks", System.StringComparison.CurrentCultureIgnoreCase))
            {
                app.UseCors("PermitirTodos");
            }
            else
            {
                app.UseCors("PermitirAmbiente");
            }
        }
    }
}
