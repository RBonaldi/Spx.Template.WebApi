using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public static class LogStartup
    {
        public static void AddLogCustomizado(this IServiceCollection servicos, IConfiguration configuracao)
        {
            servicos.AddLogging();
        }
    }
}