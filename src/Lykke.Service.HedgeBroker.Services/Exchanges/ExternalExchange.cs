using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Client;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Common.ExchangeAdapter.SpotController;
using Lykke.Common.ExchangeAdapter.SpotController.Records;
using Lykke.Common.Log;
using Lykke.Service.HedgeBroker.Core.Exchanges;
using Lykke.Service.HedgeBroker.Core.Services;
using Lykke.Service.HedgeBroker.Core.Settings;

namespace Lykke.Service.HedgeBroker.Services.Exchanges
{
    [UsedImplicitly]
    public class ExternalExchange : IExternalExchange
    {
        private readonly ExchangeAdapterClientFactory _exchangeAdapterClientFactory;
        private readonly ISettingsService _settingsService;
        private readonly ILog _log;

        public ExternalExchange(
            ExchangeAdapterClientFactory exchangeAdapterClientFactory,
            ISettingsService settingsService,
            ILogFactory logFactory)
        {
            _exchangeAdapterClientFactory = exchangeAdapterClientFactory;
            _settingsService = settingsService;
            _log = logFactory.CreateLog(this);
        }

        public async Task<OrderIdResponse> CreateLimitOrderAsync(string exchange, LimitOrderRequest limitOrderRequest)
        {
            // TODO: get a volume accuracy from external instrument settings
            int volumeAccuracy = 6;

            if (limitOrderRequest.TradeType == TradeType.Buy)
            {
                decimal fee = await GetFeeAsync(exchange);

                limitOrderRequest.Volume =
                    Math.Round(limitOrderRequest.Volume * (1 + fee), volumeAccuracy);
            }

            _log.Info("External exchange create limit order request.", new
            {
                request = $"data: {limitOrderRequest.ToJson()}",
                originalVolume = limitOrderRequest.Volume,
                exchange
            });

            OrderIdResponse response;

            try
            {
                ISpotController spotController = _exchangeAdapterClientFactory.GetSpotController(exchange);

                response = await spotController.CreateLimitOrderAsync(limitOrderRequest);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during creating limit orders", limitOrderRequest);
                throw;
            }

            _log.Info("External exchange create limit order response.", new
            {
                response = $"data: {response.ToJson()}",
                originalVolume = limitOrderRequest.Volume,
                exchange
            });

            return response;
        }

        public async Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string exchange, string limitOrderId)
        {
            var cancelLimitOrderRequest = new CancelLimitOrderRequest
            {
                OrderId = limitOrderId
            };

            _log.Info("External exchange cancel limit order request", new
            {
                request = $"data: {cancelLimitOrderRequest.ToJson()}",
                exchange
            });

            CancelLimitOrderResponse response;

            try
            {
                ISpotController spotController = _exchangeAdapterClientFactory.GetSpotController(exchange);

                response = await spotController.CancelLimitOrderAsync(cancelLimitOrderRequest);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during cancelling limit orders", new
                {
                    limitOrderId,
                    exchange
                });

                throw;
            }

            _log.Info("External exchange cancel limit order response.", new
            {
                responce = $"data: {response.ToJson()}",
                exchange
            });

            return response;
        }

        public async Task<OrderModel> GetLimitOrderAsync(string exchange, string limitOrderId)
        {
            _log.Info("External exchange get limit order status request.", new
            {
                limitOrderId,
                exchange
            });

            OrderModel orderModel;

            try
            {
                ISpotController spotController = _exchangeAdapterClientFactory.GetSpotController(exchange);

                orderModel = await spotController.LimitOrderStatusAsync(limitOrderId);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during getting limit orders", new
                {
                    limitOrderId,
                    exchange
                });

                throw;
            }

            _log.Info("External exchange get limit order status response.", new
            {
                response = $"data: {orderModel.ToJson()}",
                exchange
            });

            return orderModel;
        }

        public async Task<GetWalletsResponse> GetBalancesAsync(string exchange)
        {
            _log.Info("External exchange get balances request.", new { exchange });

            GetWalletsResponse response;

            try
            {
                ISpotController spotController = _exchangeAdapterClientFactory.GetSpotController(exchange);

                response = await spotController.GetWalletBalancesAsync();
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during getting balances", new { exchange });

                throw;
            }

            _log.Info("External exchange get balances response.", new
            {
                response = $"data: {response.ToJson()}",
                exchange
            });

            return response;
        }

        private async Task<decimal> GetFeeAsync(string exchange)
        {
            IReadOnlyList<ExternalExchangeSettings> externalExchangeSettings =
                await _settingsService.GetExternalExchangesAsync();

            return externalExchangeSettings.FirstOrDefault(o => o.Name == exchange)?.Fee ?? decimal.Zero;
        }
    }
}
