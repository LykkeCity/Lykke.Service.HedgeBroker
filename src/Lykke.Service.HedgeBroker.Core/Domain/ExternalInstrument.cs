using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.HedgeBroker.Core.Domain
{
    public class ExternalInstrument
    {
        public string Exchange { get; set; }

        public string AssetPair { get; set; }

        public string BaseAsset { get; set; }

        public string QuoteAsset { get; set; }

        public int PriceAccuracy { get; set; }

        public int VolumeAccuracy { get; set; }

        public decimal MinVolume { get; set; }

        public decimal BaseReservedVolume { get; set; }

        public decimal QuoteReservedVolume { get; set; }
    }
}
