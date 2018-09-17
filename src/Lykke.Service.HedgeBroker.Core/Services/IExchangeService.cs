using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.SpotController.Records;

namespace Lykke.Service.HedgeBroker.Core.Services
{
    public interface IExchangeService
    {
        Task<OrderIdResponse> CreateLimitOrderAsync(LimitOrderRequest limitOrderRequest);

        Task<CancelLimitOrderResponse> CancelLimitOrderAsync(string limitOrderId);

        Task<OrderModel> GetLimitOrderAsync(string limitOrderId);

        Task<GetWalletsResponse> GetBalancesAsync();
    }
}
