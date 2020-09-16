using devboost.Kafka.Pagamentos.Consumer.Enums;
using System;

namespace devboost.Kafka.Pagamentos.Consumer.Model
{
    public class Pedido
    {
        public Guid IdPedido { get; set; }
        public StatusPagamentoEnum StatusPagamento { get; set; }
    }
}