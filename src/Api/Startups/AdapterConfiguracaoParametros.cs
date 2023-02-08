using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Api
{
    public static class OtcConfigurationExtensions
    {
        public static T SafeGet<T>(this IConfiguration configuration)
        {
            var typeName = typeof(T).Name;

            if (configuration.GetChildren().Any(item => item.Key == typeName))
            {
                configuration = configuration.GetSection(typeName);
            }

            T model = configuration.Get<T>();

            if (model == null)
            {
                throw new InvalidOperationException(
                    $"Item de configuracao nao encontrado para o tipo {typeof(T).FullName}.");
            }

            return model;
        }
    }
}
