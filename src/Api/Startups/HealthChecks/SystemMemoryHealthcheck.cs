using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api
{
    public class SystemMemoryHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var client = new MemoryMetricsClient();
            var metrics = client.GetMetrics();
            var percentualUsado = 100 * metrics.Usado / metrics.Total;

            var status = HealthStatus.Healthy;
            if (percentualUsado > 80)
            {
                status = HealthStatus.Degraded;
            }
            if (percentualUsado > 90)
            {
                status = HealthStatus.Unhealthy;
            }

            var data = new Dictionary<string, object>();
            data.Add("Total", metrics.Total);
            data.Add("Usado", metrics.Usado);
            data.Add("Livre", metrics.Livre);

            var resultado = new HealthCheckResult(status, $"O uso de memória está em {percentualUsado:0.##}%", null, data);

            return await Task.FromResult(resultado);
        }
    }
}
