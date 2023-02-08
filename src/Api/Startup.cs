using Cor.IntegracaoImdb.Api.TmdbAdapter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuracao = configuration;
        }

        public IConfiguration Configuracao { get; }

        public void ConfigureServices(IServiceCollection servicos)
        {
            servicos.AddCorsCustomizado(Configuracao);

            servicos.AddDommelCustomizado();

            servicos.AddMemoryCache();

            servicos.AddHealthChecksCustomizado(Configuracao);

            servicos.AddDICustomizado();

            servicos.AddAutoMapperCustomizadoApi();

            servicos.AddAutenticacaoCustomizada(Configuracao);

            servicos.AddMvcCustomizado();

            servicos.AddLogCustomizado(Configuracao);

            servicos.AddSwaggerCustomizado(Configuracao);
            ConfigureApiServices(servicos);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorsCustomizado(env);

            app.UseExcecaoCustomizada();

            app.UseSwaggerCustomizado();

            app.UseHealthChecksCustomizado();

            app.UseMvcCustomizado(Configuracao);
        }

        private void ConfigureApiServices(IServiceCollection servicos)
        {
            
            servicos.AddTmdbAdapter(
                Configuracao.SafeGet<TmdbAdapterConfiguration>());
        }
    }
}

