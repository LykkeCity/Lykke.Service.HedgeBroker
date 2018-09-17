using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Client;
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

        public Task<OrderIdResponse> CreateLimitOrderAsync(string exchange, LimitOrderRequest limitOrderRequest)
        {
            return ExecuteAsync(
                spotController => spotController.CreateLimitOrderAsync(limitOrderRequest),
                "External exchange create limit order",
                exchange,
                new
                {
                    request = $"data: {limitOrderRequest.ToJson()}",
                    originalVolume = limitOrderRequest.Volume,
                    exchange
                });
        }

        public Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string exchange, string limitOrderId)
        {
            var cancelLimitOrderRequest = new CancelLimitOrderRequest
            {
                OrderId = limitOrderId
            };

            return ExecuteAsync(
                spotController => spotController.CancelLimitOrderAsync(cancelLimitOrderRequest),
                "External exchange cancel limit order",
                exchange,
                cancelLimitOrderRequest);
        }

        public Task<OrderModel> GetLimitOrderAsync(string exchange, string limitOrderId)
        {
            return ExecuteAsync(
                spotController => spotController.LimitOrderStatusAsync(limitOrderId),
                "External exchange get limit order status",
                exchange,
                new
                {
                    limitOrderId,
                    exchange
                });
        }

        public Task<GetLimitOrdersResponse> GetLimitOrdersAsync(string exchange)
        {
            return ExecuteAsync(
                spotController => spotController.GetLimitOrdersAsync(),
                "External exchange get limit orders",
                exchange,
                new
                {
                    exchange
                });
        }

        public Task<GetWalletsResponse> GetBalancesAsync(string exchange)
        {
            return ExecuteAsync(
                spotController => spotController.GetWalletBalancesAsync(),
                "External exchange get balances",
                exchange,
                new
                {
                    exchange
                });
        }

        public Task<OrderIdResponse> CreateMarketOrderAsync(string exchange, MarketOrderRequest request)
        {
            return ExecuteAsync(
                spotController => spotController.CreateMarketOrderAsync(request),
                "External exchange create market order",
                exchange,
                request);
        }

        public Task<OrderModel> GetMarketOrderAsync(string exchange, string orderId)
        {
            return ExecuteAsync(
                spotController => spotController.MarketOrderStatusAsync(orderId),
                "External exchange get market order status",
                exchange,
                new
                {
                    orderId
                }
            );
        }

        public Task<GetOrdersHistoryResponse> GetOrdersHistoryAsync(string exchange)
        {
            return ExecuteAsync(
                spotController => spotController.GetOrdersHistoryAsync(),
                "External exchange get orders history",
                exchange,
                new
                {
                    exchange
                });
        }

        public Task<OrderIdResponse> ReplaceLimitOrderAsync(string exchange, ReplaceLimitOrderRequest request)
        {
            return ExecuteAsync(
                spotController => spotController.ReplaceLimitOrderAsync(request),
                "External exchange replace limit order",
                exchange,
                request);
        }

        private async Task<decimal> GetFeeAsync(string exchange)
        {
            IReadOnlyList<ExternalExchangeSettings> externalExchangeSettings =
                await _settingsService.GetExternalExchangesAsync();

            return externalExchangeSettings.FirstOrDefault(o => o.Name == exchange)?.Fee ?? decimal.Zero;
        }
        
        private async Task<TResponse> ExecuteAsync<TResponse>(Func<ISpotController, Task<TResponse>> action,
            string process, string exchange, object context)
        {
            _log.Info($"{process} request", context);

            TResponse response;

            try
            {
                ISpotController spotController = _exchangeAdapterClientFactory.GetSpotController(exchange);

                response = await action(spotController);
            }
            catch (Exception exception)
            {
                _log.Error(exception, $"An error occurred during processing '{process}'", context);
                throw;
            }

            _log.Info($"{process} response", new
            {
                responce = $"data: {response.ToJson()}",
                exchange
            });

            return response;
        }

    }
}
