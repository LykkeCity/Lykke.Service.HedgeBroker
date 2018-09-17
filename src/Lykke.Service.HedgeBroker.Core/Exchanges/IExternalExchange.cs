using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.SpotController.Records;

namespace Lykke.Service.HedgeBroker.Core.Exchanges
{
    public interface IExternalExchange
    {
        Task<OrderIdResponse> CreateLimitOrderAsync(string exchange, LimitOrderRequest limitOrderRequest);

        Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string exchange, string limitOrderId);
        
        Task<OrderModel> GetLimitOrderAsync(string exchange, string limitOrderId);

        Task<GetLimitOrdersResponse> GetLimitOrdersAsync(string exchange);

        Task<GetWalletsResponse> GetBalancesAsync(string exchange);

        Task<OrderIdResponse> CreateMarketOrderAsync(string exchange, MarketOrderRequest request);

        Task<OrderModel> GetMarketOrderAsync(string exchange, string orderId);

        Task<GetOrdersHistoryResponse> GetOrdersHistoryAsync(string exchange);
        
        Task<OrderIdResponse> ReplaceLimitOrderAsync(string exchange, ReplaceLimitOrderRequest request);
    }
}
