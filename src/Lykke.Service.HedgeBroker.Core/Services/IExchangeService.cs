using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.SpotController.Records;

namespace Lykke.Service.HedgeBroker.Core.Services
{
    public interface IExchangeService
    {
        Task<OrderIdResponse> CreateLimitOrderAsync(LimitOrderRequest limitOrderRequest);

        Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string limitOrderId);

        Task<OrderIdResponse> CreateMarketOrderAsync(MarketOrderRequest request);

        Task<OrderModel> GetLimitOrderAsync(string limitOrderId);

        Task<GetLimitOrdersResponse> GetLimitOrdersAsync();

        Task<OrderModel> GetMarketOrderAsync(string orderId);

        Task<GetWalletsResponse> GetBalancesAsync();
        
        Task<GetOrdersHistoryResponse> GetOrdersHistoryAsync();
        
        Task<OrderIdResponse> ReplaceLimitOrderAsync(ReplaceLimitOrderRequest request);
    }
}
