using devboost.Kafka.Pagamentos.Consumer.Model;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

namespace devboost.Kafka.Pagamentos.Consumer.External
{
    public class DeliveryExternalControl
    {
        private readonly string _deliveryURL;

        public DeliveryExternalControl(IConfiguration configuration)
        {
            _deliveryURL = configuration.GetValue<string>("DELIVERY__URL");
        }

        public void AtualizaStatusPedido_E_ProcessaEntrega(Pedido pedido)
        {
            var url = $"{_deliveryURL}/entrega/inicia/pedido";
            url.PostJsonAsync(pedido);
        }
    }
}