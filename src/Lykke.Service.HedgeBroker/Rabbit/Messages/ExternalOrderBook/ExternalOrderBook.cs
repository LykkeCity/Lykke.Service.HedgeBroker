using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Lykke.Service.HedgeBroker.Rabbit.Messages.ExternalOrderBook
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ExternalOrderBook
    {
        public string Source { get; set; }

        [JsonProperty("asset")]
        public string AssetPairId { get; set; }

        public DateTime Timestamp { get; set; }

        [JsonProperty("asks")]
        public IReadOnlyList<ExternalLimitOrder> SellLimitOrders { get; set; }

        [JsonProperty("bids")]
        public IReadOnlyList<ExternalLimitOrder> BuyLimitOrders { get; set; }
    }
}
