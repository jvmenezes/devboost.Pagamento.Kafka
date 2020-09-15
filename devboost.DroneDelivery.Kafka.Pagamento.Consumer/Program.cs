using devboost.Kafka.Pagamentos.Consumer;
using Microsoft.Extensions.DependencyInjection;

namespace devboost.DroneDelivery.Kafka.Pagamento.Consumer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            Startup.ConfigureServices(services);
            services.BuildServiceProvider().GetService<ConsoleAtualizaPagamentoPedido>().Run();
        }
    }
}