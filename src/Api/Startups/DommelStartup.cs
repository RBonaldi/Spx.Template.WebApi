using Dommel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Api
{
    public static class DommelStartup
    {
        public static void AddDommelCustomizado(this IServiceCollection services) => DommelMapper.SetTableNameResolver(new TabelaResolver());

        public class TabelaResolver : ITableNameResolver
        {
            public string ResolveTableName(Type type)
            {
                var typeInfo = type.GetTypeInfo();
                var tableAttr = typeInfo.GetCustomAttribute<TableAttribute>();
                if (tableAttr != null)
                {
                    return tableAttr.Name;
                }
                return $"{type.Name}";
            }
        }
    }
}
