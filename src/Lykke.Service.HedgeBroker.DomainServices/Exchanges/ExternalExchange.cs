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
using Lykke.Service.HedgeBroker.Domain.Exchanges;
using Lykke.Service.HedgeBroker.Domain.Services;
using Lykke.Service.HedgeBroker.Domain.Settings;

namespace Lykke.Service.HedgeBroker.DomainServices.Exchanges
{
    [UsedImplicitly]
    public class ExternalExchange : IExternalExchange
    {
        private readonly ExchangeAdapterClientFactory _exchangeAdapterClientFactory;
        private readonly ISettingsService _settingsService;
        private readonly string _exchangeName;
        private readonly ILog _log;

        public ExternalExchange(
            ExchangeAdapterClientFactory exchangeAdapterClientFactory,
            ISettingsService settingsService,
            string exchangeName,
            ILogFactory logFactory)
        {
            _exchangeAdapterClientFactory = exchangeAdapterClientFactory;
            _settingsService = settingsService;
            _exchangeName = exchangeName;
            _log = logFactory.CreateLog(this);
        }

        public Task<OrderIdResponse> CreateLimitOrderAsync(LimitOrderRequest limitOrderRequest)
        {
            return ExecuteAsync(
                spotController => spotController.CreateLimitOrderAsync(limitOrderRequest),
                "External exchange create limit order",
                new
                {
                    request = $"data: {limitOrderRequest.ToJson()}",
                    originalVolume = limitOrderRequest.Volume,
                    _exchangeName
                });
        }

        public Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string limitOrderId)
        {
            var cancelLimitOrderRequest = new CancelLimitOrderRequest
            {
                OrderId = limitOrderId
            };

            return ExecuteAsync(
                spotController => spotController.CancelLimitOrderAsync(cancelLimitOrderRequest),
                "External exchange cancel limit order",
                cancelLimitOrderRequest);
        }

        public Task<OrderModel> GetLimitOrderAsync(string limitOrderId)
        {
            return ExecuteAsync(
                spotController => spotController.LimitOrderStatusAsync(limitOrderId),
                "External exchange get limit order status",
                new
                {
                    limitOrderId,
                    _exchangeName
                });
        }

        public Task<GetLimitOrdersResponse> GetLimitOrdersAsync()
        {
            return ExecuteAsync(
                spotController => spotController.GetLimitOrdersAsync(),
                "External exchange get limit orders",
                new
                {
                    _exchangeName
                });
        }

        public Task<GetWalletsResponse> GetBalancesAsync()
        {
            return ExecuteAsync(
                spotController => spotController.GetWalletBalancesAsync(),
                "External exchange get balances",
                new
                {
                    _exchangeName
                });
        }

        public Task<OrderIdResponse> CreateMarketOrderAsync(MarketOrderRequest request)
        {
            return ExecuteAsync(
                spotController => spotController.CreateMarketOrderAsync(request),
                "External exchange create market order",
                request);
        }

        public Task<OrderModel> GetMarketOrderAsync(string orderId)
        {
            return ExecuteAsync(
                spotController => spotController.MarketOrderStatusAsync(orderId),
                "External exchange get market order status",
                new
                {
                    orderId,
                    _exchangeName
                }
            );
        }

        public Task<GetOrdersHistoryResponse> GetOrdersHistoryAsync()
        {
            return ExecuteAsync(
                spotController => spotController.GetOrdersHistoryAsync(),
                "External exchange get orders history",
                new
                {
                    _exchangeName
                });
        }

        public Task<OrderIdResponse> ReplaceLimitOrderAsync(ReplaceLimitOrderRequest request)
        {
            return ExecuteAsync(
                spotController => spotController.ReplaceLimitOrderAsync(request),
                "External exchange replace limit order",
                request);
        }

        private async Task<decimal> GetFeeAsync()
        {
            IReadOnlyList<ExternalExchangeSettings> externalExchangeSettings =
                await _settingsService.GetExternalExchangesAsync();

            return externalExchangeSettings.FirstOrDefault(o => o.Name == _exchangeName)?.Fee ?? decimal.Zero;
        }
        
        private async Task<TResponse> ExecuteAsync<TResponse>(Func<ISpotController, Task<TResponse>> action,
            string process, object context)
        {
            _log.Info($"{process} request", context);

            TResponse response;

            try
            {
                ISpotController spotController = _exchangeAdapterClientFactory.GetSpotController(_exchangeName);

                response = await action(spotController);
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"An error occurred during processing '{process}'", context);
                throw;
            }

            _log.Info($"{process} response", new
            {
                responce = $"data: {response.ToJson()}",
                _exchangeName
            });

            return response;
        }

    }
}
