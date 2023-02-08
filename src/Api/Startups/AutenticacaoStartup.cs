using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    /// <summary>
    /// Autenticação.
    /// </summary>
    public static class AutenticacaoStartup
    {
        public static void AddAutenticacaoCustomizada(this IServiceCollection servicos, IConfiguration configuracao)
        {
            servicos.AddAuthentication(opcoes =>
            {
                opcoes.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opcoes.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opcoes =>
            {
                opcoes.Authority = configuracao.GetSection("Autenticacao:IdentityServer:UrlBase").Value;
                opcoes.Audience = "apisupermix";
#if DEBUG
                opcoes.RequireHttpsMetadata = false;
#endif
            });

            servicos.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            });
        }

        public static void UseAutenticacaoCustomizada(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
