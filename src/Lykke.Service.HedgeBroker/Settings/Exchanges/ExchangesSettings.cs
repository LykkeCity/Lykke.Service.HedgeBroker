using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Settings.Exchanges.External;

namespace Lykke.Service.HedgeBroker.Settings.Exchanges
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ExchangesSettings
    {
        public ExternalExchangeSettings[] External { get; set; }
    }
}
