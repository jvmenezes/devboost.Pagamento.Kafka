using System.Threading.Tasks;
using Devboost.Pagamentos.Domain.Interfaces.External.Context;
using Devboost.Pagamentos.Domain.Params;
using Devboost.Pagamentos.Domain.VO;
using Flurl.Http;

namespace Devboost.Pagamentos.External.Context
{
    public class DeliveryExternalContext: IDeliveryExternalContext
    {
        private readonly ExternalConfigVO _config;

        public DeliveryExternalContext(ExternalConfigVO config)
        {
            _config = config;
        }

        public virtual async Task AtualizaStatusPagamento(DeliveryExternalParam deliverypParam)
        {
            //var url = $"{_config.DeliveryUrl}/entrega/inicia/pedido";
            var url = $"{_config.ProducerPagamentoUrl}/producerpagamento/statuspagamento"; //Chamando a API Producer Pagamento para indexar essa informação de status como tópico no Kafka
            await url.PostJsonAsync(deliverypParam)
                .ConfigureAwait(false);
        }
    }
}