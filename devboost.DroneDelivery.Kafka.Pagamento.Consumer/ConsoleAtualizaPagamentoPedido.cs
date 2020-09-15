using Confluent.Kafka;
using devboost.Kafka.Pagamentos.Consumer.External;
using devboost.Kafka.Pagamentos.Consumer.Model;
using devboost.Kafka.Pagamentos.Consumer.Validators;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using System;
using System.Text.Json;
using System.Threading;

namespace devboost.Kafka.Pagamentos.Consumer
{
    public class ConsoleAtualizaPagamentoPedido
    {
        private readonly Logger _logger;
        private readonly IConfiguration _configuration;
        private readonly DeliveryExternalControl _deliveryExternalControl;

        public ConsoleAtualizaPagamentoPedido(Logger logger, IConfiguration configuration, DeliveryExternalControl deliveryExternalControl)
        {
            _logger = logger;
            _configuration = configuration;
            _deliveryExternalControl = deliveryExternalControl;
        }

        public void Run()
        {
            _logger.Information("Testando o consumo de mensagens com Kafka para atualização de Status do Pedido e iniciação da entrega via Drone, caso validações estejam aptas");
            string nomeTopic = _configuration["Kafka_Topic_II"];
            _logger.Information($"Topic = {nomeTopic}");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                using (var consumer = GetConsumerBuilder())
                {
                    consumer.Subscribe(nomeTopic);

                    try
                    {
                        while (true)
                        {
                            var cr = consumer.Consume(cts.Token);
                            string dados = cr.Message.Value;

                            _logger.Information(
                                $"Mensagem lida: {dados}");

                            var pedido = JsonSerializer.Deserialize<Pedido>(dados,
                                new JsonSerializerOptions()
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                            var validationResult = new PedidoValidator().Validate(pedido);
                            if (validationResult.IsValid)
                            {
                                _deliveryExternalControl.AtualizaStatusPedido_E_ProcessaEntrega(pedido);
                                _logger.Information("Atualização de Status do pedido e processamento da entrega registrada com sucesso!");
                            }
                            else
                                _logger.Error("Dados inválidos para a atualização de pedido e processamento de entrega");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                        _logger.Warning("Cancelada a execução do Consumer AtualizaStatusPedido_E_ProcessaEntrega...");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exceção: {ex.GetType().FullName} | " +
                             $"Mensagem: {ex.Message}");
            }
        }

        private IConsumer<Ignore, string> GetConsumerBuilder()
        {
            string bootstrapServers = _configuration["Kafka_Broker"];
            _logger.Information($"BootstrapServers = {bootstrapServers}");

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = $"consoleapp-AtualizaPagamentoPedido",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _logger.Information($"GroupId = {config.GroupId}");

            return new ConsumerBuilder<Ignore, string>(config).Build();
        }
    }
}