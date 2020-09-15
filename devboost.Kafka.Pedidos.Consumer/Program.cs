using Microsoft.Extensions.DependencyInjection;

namespace devboost.Kafka.Pedidos.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            Startup.ConfigureServices(services);
            services.BuildServiceProvider().GetService<ConsoleFazPedido>().Run();
        }
    }
}