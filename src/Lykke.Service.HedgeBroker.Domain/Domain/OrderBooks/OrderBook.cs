using System;
using System.Collections.Generic;

namespace Lykke.Service.HedgeBroker.Domain.Domain.OrderBooks
{
    public class OrderBook
    {
        public string Exchange { get; set; }

        public string AssetPairId { get; set; }

        public DateTime Timestamp { get; set; }

        public IReadOnlyList<OrderBookLimitOrder> SellLimitOrders { get; set; }

        public IReadOnlyList<OrderBookLimitOrder> BuyLimitOrders { get; set; }
    }
}
