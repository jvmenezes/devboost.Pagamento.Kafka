﻿using System;
using System.Threading.Tasks;
using Devboost.DroneDelivery.Domain.Entities;
using Devboost.DroneDelivery.Domain.Interfaces.Repository;
using Devboost.DroneDelivery.Domain.Interfaces.Services;
using Devboost.DroneDelivery.Domain.Params;

namespace Devboost.DroneDelivery.DomainService
{
    public class PedidoService: IPedidoService
    {
        private readonly IDroneService _droneService;
        private readonly IPedidosRepository _pedidosRepository;

        public PedidoService(IDroneService droneService, IPedidosRepository pedidosRepository)
        {
            _droneService = droneService;
            _pedidosRepository = pedidosRepository;
        }

        public async Task<bool> InserirPedido(PedidoParam pedido)
        {
            var novoPedido = new PedidoEntity
            {
                PesoGramas = pedido.Peso,
                Latitude = pedido.Latitude,
                Longitude = pedido.Longitude,
                DataHora = pedido.DataHora,
                };
            
            //calculoDistancia

            var distancia = GeolocalizacaoService.CalcularDistanciaEmMetro(pedido.Latitude,pedido.Longitude);
            
            if (!novoPedido.ValidaPedido(distancia))
                return false;

            var drone = await _droneService.SelecionarDrone();
            novoPedido.Drone = drone;
            novoPedido.DroneId = drone.Id;
           // await _pedidosRepository.Inserir(novoPedido);
            await _droneService.AtualizaDrone(drone);
            
           return true;
        }

        public async Task<Guid> IdPedidoPorIdDrone(int DroneId)
        {
            throw new NotImplementedException();
        }
    }
}