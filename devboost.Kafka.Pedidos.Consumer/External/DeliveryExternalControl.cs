﻿using devboost.Kafka.Pedidos.Consumer.DTO;
using devboost.Kafka.Pedidos.Consumer.Model;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

namespace devboost.Kafka.Pedidos.Consumer.External
{
    public class DeliveryExternalControl
    {
        private readonly string _deliveryURL;

        public DeliveryExternalControl(IConfiguration configuration)
        {
            _deliveryURL = configuration.GetValue<string>("DELIVERY__URL");
        }

        public TokenDTO Logar(Auth auth)
        {
            var url = $"{_deliveryURL}/auth";

            var result = url.PostJsonAsync(auth).ReceiveJson<TokenDTO>();

            result.Wait();

            return result.Result;
        }

        public void EnviarPedido(Pedido pedido, TokenDTO token)
        {
            var url = $"{_deliveryURL}/pedido/cadastrar";
            url.WithOAuthBearerToken(token.AccessToken).PostJsonAsync(pedido);
        }
    }
}