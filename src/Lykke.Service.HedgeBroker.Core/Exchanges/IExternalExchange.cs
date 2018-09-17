using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.SpotController.Records;

namespace Lykke.Service.HedgeBroker.Core.Exchanges
{
    public interface IExternalExchange
    {
        Task<OrderIdResponse> CreateLimitOrderAsync(string exchange, LimitOrderRequest limitOrderRequest);

        Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string exchange, string limitOrderId);
        
        Task<OrderModel> GetLimitOrderAsync(string exchange, string limitOrderId);

        Task<GetWalletsResponse> GetBalancesAsync(string exchange);
    }
}
