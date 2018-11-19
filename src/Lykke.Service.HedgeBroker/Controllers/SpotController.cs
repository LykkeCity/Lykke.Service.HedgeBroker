using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.ExchangeAdapter.SpotController;
using Lykke.Common.ExchangeAdapter.SpotController.Records;
using Lykke.Common.Log;
using Lykke.Service.HedgeBroker.Domain.Exchanges;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.HedgeBroker.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class SpotController : Controller, ISpotController
    {
        private readonly IExternalExchange _exchangeService;
        private readonly ILog _log;

        public SpotController(
            IExternalExchange exchangeService,
            ILogFactory logFactory)
        {
            _exchangeService = exchangeService;
            _log = logFactory.CreateLog(this);
        }

        [HttpPost("cancelOrder")]
        [ProducesResponseType(typeof(CancelLimitOrderResponse), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public Task<CancelLimitOrderResponse> CancelLimitOrderAsync([FromBody]CancelLimitOrderRequest request)
        {
            return _exchangeService.CancelLimitOrderAsync(request.OrderId);
        }

        [HttpPost("createLimitOrder")]
        [ProducesResponseType(typeof(OrderIdResponse), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public Task<OrderIdResponse> CreateLimitOrderAsync([FromBody]LimitOrderRequest request)
        {
            return _exchangeService.CreateLimitOrderAsync(request);
        }

        [HttpPost("createMarketOrder")]
        [ProducesResponseType(typeof(OrderIdResponse), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public Task<OrderIdResponse> CreateMarketOrderAsync([FromBody]MarketOrderRequest request)
        {
            return _exchangeService.CreateMarketOrderAsync(request);
        }

        [HttpGet("getLimitOrders")]
        [ProducesResponseType(typeof(GetLimitOrdersResponse), 200)]
        public Task<GetLimitOrdersResponse> GetLimitOrdersAsync()
        {
            return _exchangeService.GetLimitOrdersAsync();
        }

        [HttpGet("getOrdersHistory")]
        [ProducesResponseType(typeof(GetOrdersHistoryResponse), 200)]
        public Task<GetOrdersHistoryResponse> GetOrdersHistoryAsync()
        {
            return _exchangeService.GetOrdersHistoryAsync();
        }

        [HttpGet("getWallets")]
        [ProducesResponseType(typeof(GetWalletsResponse), 200)]
        public Task<GetWalletsResponse> GetWalletBalancesAsync()
        {
            return _exchangeService.GetBalancesAsync();
        }

        [HttpGet("limitOrderStatus")]
        [ProducesResponseType(typeof(OrderModel), 200)]
        public Task<OrderModel> LimitOrderStatusAsync(string orderId)
        {
            return _exchangeService.GetLimitOrderAsync(orderId);
        }
        
        [HttpGet("marketOrderStatus")]
        [ProducesResponseType(typeof(OrderModel), 200)]
        public Task<OrderModel> MarketOrderStatusAsync(string orderId)
        {
            return _exchangeService.GetMarketOrderAsync(orderId);
        }
        
        [HttpPost("replaceLimitOrder")]
        [ProducesResponseType(typeof(OrderIdResponse), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public Task<OrderIdResponse> ReplaceLimitOrderAsync([FromBody] ReplaceLimitOrderRequest request)
        {
            return _exchangeService.ReplaceLimitOrderAsync(request);
        }
    }
}
