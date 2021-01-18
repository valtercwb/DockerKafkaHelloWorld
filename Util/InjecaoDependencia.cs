using Microsoft.Extensions.DependencyInjection;

namespace KafkaDockerHelloWorld.Util
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection ConfigurarDependencias(this IServiceCollection services)
        {
            return services;
        }
    }
}