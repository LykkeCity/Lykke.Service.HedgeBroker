using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.ExchangeAdapter.SpotController;
using Lykke.Common.ExchangeAdapter.SpotController.Records;
using Lykke.Common.Log;
using Lykke.Service.HedgeBroker.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.HedgeBroker.Controllers
{
    [Produces("application/json")]
    [Route("/api/[controller]")]
    public class SpotController : Controller, ISpotController
    {
        private readonly IExchangeService _exchangeService;
        private readonly ILog _log;

        public SpotController(
            IExchangeService exchangeService,
            ILogFactory logFctory)
        {
            _exchangeService = exchangeService;
            _log = logFctory.CreateLog(this);
        }

        [SwaggerOperation("CancelOrder")]
        [HttpPost("cancelOrder")]
        [ProducesResponseType(typeof(CancelLimitOrderResponse), 200)]
        public Task<CancelLimitOrderResponse> CancelLimitOrderAsync([FromBody]CancelLimitOrderRequest request)
        {
            return _exchangeService.CancelLimitOrderAsync(request.OrderId);
        }

        [SwaggerOperation("CreateLimitOrder")]
        [HttpPost("createLimitOrder")]
        [ProducesResponseType(typeof(OrderIdResponse), 200)]
        public Task<OrderIdResponse> CreateLimitOrderAsync([FromBody]LimitOrderRequest request)
        {
            return _exchangeService.CreateLimitOrderAsync(request);
        }

        [SwaggerOperation("CreateMarketOrder")]
        [HttpPost("createMarketOrder")]
        [ProducesResponseType(typeof(OrderIdResponse), 200)]
        public Task<OrderIdResponse> CreateMarketOrderAsync([FromBody]MarketOrderRequest request)
        {
            return _exchangeService.CreateMarketOrderAsync(request);
        }

        [SwaggerOperation("GetLimitOrders")]
        [HttpGet("getLimitOrders")]
        [ProducesResponseType(typeof(GetLimitOrdersResponse), 200)]
        public Task<GetLimitOrdersResponse> GetLimitOrdersAsync()
        {
            return _exchangeService.GetLimitOrdersAsync();
        }

        [SwaggerOperation("GetOrdersHistory")]
        [HttpGet("getOrdersHistory")]
        [ProducesResponseType(typeof(GetOrdersHistoryResponse), 200)]
        public Task<GetOrdersHistoryResponse> GetOrdersHistoryAsync()
        {
            return _exchangeService.GetOrdersHistoryAsync();
        }

        [SwaggerOperation("GetWalletBalances")]
        [HttpGet("getWallets")]
        [ProducesResponseType(typeof(GetWalletsResponse), 200)]
        public Task<GetWalletsResponse> GetWalletBalancesAsync()
        {
            return _exchangeService.GetBalancesAsync();
        }

        [SwaggerOperation("LimitOrderStatus")]
        [HttpGet("limitOrderStatus")]
        [ProducesResponseType(typeof(OrderModel), 200)]
        public Task<OrderModel> LimitOrderStatusAsync(string orderId)
        {
            return _exchangeService.GetLimitOrderAsync(orderId);
        }
        
        [SwaggerOperation("MarketOrderStatus")]
        [HttpGet("marketOrderStatus")]
        [ProducesResponseType(typeof(OrderModel), 200)]
        public Task<OrderModel> MarketOrderStatusAsync(string orderId)
        {
            return _exchangeService.GetMarketOrderAsync(orderId);
        }
        
        [SwaggerOperation("ReplaceLimitOrder")]
        [HttpPost("replaceLimitOrder")]
        [ProducesResponseType(typeof(OrderIdResponse), 200)]
        public Task<OrderIdResponse> ReplaceLimitOrderAsync([FromBody] ReplaceLimitOrderRequest request)
        {
            return _exchangeService.ReplaceLimitOrderAsync(request);
        }
    }
}
