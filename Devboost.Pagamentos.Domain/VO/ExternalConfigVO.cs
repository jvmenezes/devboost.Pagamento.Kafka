namespace Devboost.Pagamentos.Domain.VO
{
    public class ExternalConfigVO
    {
        public string GatewayUrl { get; set; }
        public string DeliveryUrl { get; set; }
        public string ProducerPagamentoUrl { get; set; }
    }
}