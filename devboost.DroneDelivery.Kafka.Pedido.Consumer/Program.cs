using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace devboost.DroneDelivery.Kafka.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            Startup.ConfigureServices(services);
            services.BuildServiceProvider().GetService<ConsoleAtualizaPagamentoPedido>().Run();
        }
    }
}