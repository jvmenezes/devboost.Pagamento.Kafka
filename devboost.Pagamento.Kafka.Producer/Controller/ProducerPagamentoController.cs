using Confluent.Kafka;
using devboost.Pagamento.Kafka.Producer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace devboost.Pagamento.Kafka.Producer.Controller
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ProducerPagamentoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProducerPagamentoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("statuspagamento")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult PostStatusPagamento([FromBody] Pedido pedido)
        {
            return Created("", SendMessageByKafkaStatusPedido(pedido));
        }

        private string SendMessageByKafkaStatusPedido(Pedido pedido)
        {
            var values = JsonSerializer.Serialize(pedido);

            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var sendResult = producer
                                        .ProduceAsync(_configuration["Kafka_Topic_II"], new Message<Null, string> { Value = values })
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