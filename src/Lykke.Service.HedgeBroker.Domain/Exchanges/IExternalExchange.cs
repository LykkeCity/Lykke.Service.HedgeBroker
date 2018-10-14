using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.SpotController.Records;

namespace Lykke.Service.HedgeBroker.Domain.Exchanges
{
    public interface IExternalExchange
    {
        Task<OrderIdResponse> CreateLimitOrderAsync(LimitOrderRequest limitOrderRequest);

        Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string limitOrderId);
        
        Task<OrderModel> GetLimitOrderAsync(string limitOrderId);

        Task<GetLimitOrdersResponse> GetLimitOrdersAsync();

        Task<GetWalletsResponse> GetBalancesAsync();

        Task<OrderIdResponse> CreateMarketOrderAsync(MarketOrderRequest request);

        Task<OrderModel> GetMarketOrderAsync(string orderId);

        Task<GetOrdersHistoryResponse> GetOrdersHistoryAsync();
        
        Task<OrderIdResponse> ReplaceLimitOrderAsync(ReplaceLimitOrderRequest request);
    }
}
