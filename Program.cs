using DockerKafkaTutorial;
using FluentScheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;

namespace DockerKafkaHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Configuracoes appsettings
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                 .AddEnvironmentVariables();

            var Configuration = builder.Build();
            #endregion

            #region Configura o schedule
            var registry = new Registry();
            var intervaloMensagem = Convert.ToInt32(Configuration.GetSection("IntervaloMensagem").Value);
            registry.Schedule<EnviarMensagens>().ToRunNow().AndEvery(intervaloMensagem).Minutes();
            #endregion

            JobManager.Initialize(registry);

            Thread.Sleep(Timeout.Infinite);
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
            .ConfigureServices((Context, collection) =>
            {
                collection.AddHostedService<KafkaConsumerHostedService>();
                collection.AddHostedService<KafkaProducerHostedService>();
            });

        public class EnviarMensagens : IJob
        {
            public void Execute()
            {
                CreateHostBuilder().Build().Run();
            }
        }
    }
}