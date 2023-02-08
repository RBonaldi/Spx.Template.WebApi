using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Api
{
    public static class SwaggerStartup
    {
        public static void AddSwaggerCustomizado(this IServiceCollection servicos, IConfiguration configuracao)
        {
            servicos.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Supermix: Integração Totvs Api", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(x => x.FullName);
                c.OperationFilter<OAuth2ActionFilter>();
                c.OperationFilter<NomeCustomizadoControllerFilter>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OrderActionsBy(x => { x.TryGetMethodInfo(out var info); return info.DeclaringType.GetCustomAttributes().OfType<DescriptionAttribute>()?.FirstOrDefault()?.Description; });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{configuracao.GetSection("Autenticacao:IdentityServer:UrlBase").Value}/connect/authorize"),
                            TokenUrl = new Uri($"{configuracao.GetSection("Autenticacao:IdentityServer:UrlBase").Value}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "apisupermix", "Escopo padrão de APIs Supermix" }
                            }
                        }
                    }
                });
            });
        }

        public static IApplicationBuilder UseSwaggerCustomizado(this IApplicationBuilder app)
        {
            app.UseSwagger(o => {
                o.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "doc";
                c.OAuthAppName("API Integração Tmdb - Swagger");
                c.OAuthClientId("swaggerimplicit");
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "API REST v1");
                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }

    #region Operation Filters

    /// <summary>
    /// Filtro para customizar o nome do controller/recurso no Swagger
    /// </summary>
    public class NomeCustomizadoControllerFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out var info);
            var descricao = info.DeclaringType.GetCustomAttributes().OfType<DescriptionAttribute>()?.FirstOrDefault();

            if (descricao != null)
                operation.Tags[0] = new OpenApiTag()
                {
                    Name = descricao.Description,
                    Description = descricao.Description
                };
        }
    }

    /// <summary>
    /// Filtro para incluir segurança oauth2 nas actions necessários no Swagger
    /// </summary>
    public class OAuth2ActionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.TryGetMethodInfo(out var mi)) return;

            var found = false;
            if (context.ApiDescription.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controller)
                found = controller.ControllerTypeInfo.GetCustomAttributes().OfType<AuthorizeAttribute>().Any();

            if (!found)
                if (!mi.GetCustomAttributes().OfType<AuthorizeAttribute>().Any()) return;

            if (mi.GetCustomAttributes().OfType<AllowAnonymousAttribute>().Any()) return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [ oAuthScheme ] = new [] { "apisupermix" }
                }
            };
        }
    }

    #endregion
}
