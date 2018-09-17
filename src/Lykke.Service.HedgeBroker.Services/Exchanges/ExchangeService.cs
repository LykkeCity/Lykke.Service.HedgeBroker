using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.SpotController.Records;
using Lykke.Service.HedgeBroker.Core.Exchanges;
using Lykke.Service.HedgeBroker.Core.Services;

namespace Lykke.Service.HedgeBroker.Services.Exchanges
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExternalExchange _externalExchange;
        private readonly string _exchange;

        public ExchangeService(
            string exchange,
            IExternalExchange externalExchange)
        {
            _exchange = exchange;
            _externalExchange = externalExchange;
        }

        public Task<OrderIdResponse> CreateLimitOrderAsync(LimitOrderRequest limitOrderRequest)
        {
            return _externalExchange.CreateLimitOrderAsync(_exchange, limitOrderRequest);
        }

        public Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string limitOrderId)
        {
            return _externalExchange.CancelLimitOrderAsync(_exchange, limitOrderId);
        }

        public Task<OrderModel> GetLimitOrderAsync(string limitOrderId)
        {
            return _externalExchange.GetLimitOrderAsync(_exchange, limitOrderId);
        }

        public Task<GetWalletsResponse> GetBalancesAsync()
        {
            return _externalExchange.GetBalancesAsync(_exchange);
        }
    }
}
