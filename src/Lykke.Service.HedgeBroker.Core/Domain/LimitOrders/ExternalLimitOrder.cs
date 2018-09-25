using System;

namespace Lykke.Service.HedgeBroker.Core.Domain.LimitOrders
{
    public class ExternalLimitOrder
    {
        public string Id { get; set; }

        public string Exchange { get; set; }

        public string AssetPairId { get; set; }

        public string BaseAssetId { get; set; }

        public string QuoteAssetId { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal OriginalVolume { get; set; }

        public LimitOrderType Type { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal ExecutedVolume { get; set; }

        public LimitOrderStatus Status { get; set; }

        public decimal AvgExecutionPrice { get; set; }

        public decimal Commission { get; set; }

        public decimal ExchangeExecuteVolume { get; set; }
    }
}
