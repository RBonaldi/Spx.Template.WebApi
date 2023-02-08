using ExceptionsLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Api
{
    public static class ExcecaoStartup
    {
        public static void UseExcecaoCustomizada(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async contexto =>
                {
                    var contextoErro = contexto.Features.Get<IExceptionHandlerFeature>();
                    if (contextoErro == null)
                        return;

                    var resultado = new ExcecaoModel();

                    if (contextoErro.Error is RegistroNaoEncontradoExcecao)
                    {
                        resultado.HttpStatus = (int)HttpStatusCode.NotFound;
                        resultado.Mensagens.Add(contextoErro.Error.GetBaseException().Message);
                    }
                    else if (contextoErro.Error is ListaSemRegistrosExcecao)
                    {
                        resultado.HttpStatus = (int)HttpStatusCode.NoContent;
                        resultado.Mensagens.Add(contextoErro.Error.GetBaseException().Message);
                    }
                    else if (contextoErro.Error is ValidacaoFluentExcecao)
                    {
                        resultado.HttpStatus = (int)HttpStatusCode.BadRequest;
                        resultado.Mensagens.AddRange(((ValidacaoFluentExcecao)contextoErro.Error).Erros);
                    }
                    else if (contextoErro.Error is BaseExcecao)
                    {
                        resultado.HttpStatus = (int)HttpStatusCode.BadRequest;
                        resultado.Mensagens.Add(contextoErro.Error.GetBaseException().Message);
                    }
                    else if (contextoErro.Error is UnauthorizedAccessException)
                    {
                        resultado.HttpStatus = (int)HttpStatusCode.Unauthorized;
                        resultado.Mensagens.Add("Acesso não autorizado");
                    }
                    else
                    {
                        resultado.HttpStatus = (int)HttpStatusCode.InternalServerError;
                        try
                        {
                            var internalError = ((CoreException)(contextoErro)?.Error)?.Errors;

                            if (internalError != null)
                            {
                                resultado.HttpStatus = (int)HttpStatusCode.InternalServerError;

                                foreach (var item in internalError)
                                {
                                    resultado.CodigoErro.Add((item)?.Key);
                                    resultado.Mensagens.Add((item)?.Message);
                                }
                            }
                            else
                                resultado.Mensagens.Add(contextoErro.Error.Message);
                        }
                        catch (Exception)
                        {
                            resultado.Mensagens.Add(contextoErro.Error.Message);
                        }
                    }

                    resultado.Detalhes = contextoErro.Error.StackTrace;

                    contexto.Response.StatusCode = resultado.HttpStatus;
                    contexto.Response.ContentType = "application/json";

                    await contexto.Response.WriteAsync(JsonConvert.SerializeObject(resultado));
                });
            });
        }
    }
}
