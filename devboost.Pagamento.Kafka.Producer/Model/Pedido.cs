using Devboost.Pagamento.Kafka.Producer.Enums;
using System;

namespace devboost.Pagamento.Kafka.Producer.Model
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public StatusPagamentoEnum StatusPagamento { get; set; }
    }
}