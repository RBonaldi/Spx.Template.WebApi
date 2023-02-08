using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartialResponse.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Api
{
    public static class MvcStartup
    {
        public static void AddMvcCustomizado(this IServiceCollection servicos)
        {
            servicos.AddControllers()
                .AddNewtonsoftJson()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            servicos.Configure<MvcPartialJsonOptions>(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.IgnoreCase = true;
            });

            servicos.AddApplicationInsightsTelemetry();

            servicos.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            servicos.AddApiVersioning();
        }

        public static void UseMvcCustomizado(this IApplicationBuilder app, IConfiguration configuracao)
        {
            app.UseForwardedHeaders();

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.Use((context, next) =>
            {
                var diretorioBase = configuracao.GetSection("ASPNet:DiretorioBase")?.Value;

                if (string.IsNullOrWhiteSpace(diretorioBase))
                    return next();

                if (context.Request.Path.StartsWithSegments($"/{diretorioBase}", out var remainder))
                    context.Request.Path = remainder;

                return next();
            });

            app.UseAutenticacaoCustomizada();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
