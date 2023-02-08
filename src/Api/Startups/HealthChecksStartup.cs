using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace Api
{
    public static class HealthChecksStartup
    {
        public static void AddHealthChecksCustomizado(this IServiceCollection servicos, IConfiguration configuracao)
        {
            servicos
                .AddHealthChecks()

            #region Recursos fisicos
                .AddDiskStorageHealthCheck(
                    setup: setup => { setup.AddDrive("C:\\", 1024); },
                    name: "Espaço Mínimo Disco",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new List<string>(new[] { "storage" }))
                .AddDiskStorageHealthCheck(
                    setup: setup => { setup.AddDrive("C:\\", 0); },
                    name: "Disco Cheio",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                    tags: new List<string>(new[] { "storage" }))
                .AddCheck<SystemMemoryHealthCheck>(
                    name: "Memoria Total Maquina",
                    tags: new List<string>(new[] { "memoria" }))
                .AddWorkingSetHealthCheck(
                    maximumMemoryBytes: 629145600L,
                    name: "WorkingSet",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new List<string>(new[] { "memory" }))
                .AddVirtualMemorySizeHealthCheck(
                    maximumMemoryBytes: 629145600L,
                    name: "VirtualMemorySize",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new List<string>(new[] { "memory" }))
                .AddProcessAllocatedMemoryHealthCheck(
                    maximumMegabytesAllocated: 600,
                    name: "ProcessAllocatedMemory",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new List<string>(new[] { "memory" }))
                .AddPrivateMemoryHealthCheck(
                     maximumMemoryBytes: 629145600L,
                    name: "PrivateMemory",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new List<string>(new[] { "memory" }))
            #endregion

            #region Bases de dados
            #endregion

            #region Autenticacao
            #endregion
            #region Apis Supermix
            #endregion
                .AddApplicationInsightsPublisher(saveDetailedReport: true);

            servicos.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Period = TimeSpan.FromHours(1);
            });

            servicos.AddHealthChecksUI(setupSettings: (settings) =>
            {
                settings.SetEvaluationTimeInSeconds(86400);
            }).AddInMemoryStorage();
        }

        public static void UseHealthChecksCustomizado(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI();
        }
    }
}
