using Confluent.Kafka;
using devboost.Pagamento.Kafka.Producer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace devboost.Pagamento.Kafka.Producer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerPagamentoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProducerPagamentoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("producer/statuspagamento")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult PostStatusPagamento([FromBody] Pedido pedido)
        {
            return Created("", SendMessageByKafkaStatusPedido(pedido));
        }

        private string SendMessageByKafkaStatusPedido(Pedido pedido)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (var producer = new ProducerBuilder<Guid, Pedido>(config).Build())
            {
                try
                {
                    var sendResult = producer
                                        .ProduceAsync(_configuration["Kafka_Topic_II"], new Message<Guid, Pedido> { Key = pedido.Id, Value = pedido })
                                            .GetAwaiter()
                                                .GetResult();

                    return $"Mensagem referente ao PedidoId: '{sendResult.Key}' de '{sendResult.TopicPartitionOffset}'";
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Atualização de Status Pagamento do pedido falhou!: {e.Error.Reason}");
                }
            }

            return string.Empty;
        }
    }
}