using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lykke.Service.HedgeBroker.Core.Domain.OrderBooks
{
    public class OrderBook
    {
        public string Exchange { get; set; }

        [JsonProperty("asset")]
        public string AssetPairId { get; set; }

        public DateTime Timestamp { get; set; }

        [JsonProperty("asks")]
        public IReadOnlyList<OrderBookLimitOrder> SellLimitOrders { get; set; }

        [JsonProperty("bids")]
        public IReadOnlyList<OrderBookLimitOrder> BuyLimitOrders { get; set; }
    }
}
