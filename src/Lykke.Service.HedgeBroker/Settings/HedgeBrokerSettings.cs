using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Settings.Exchanges;

namespace Lykke.Service.HedgeBroker.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class HedgeBrokerSettings
    {
        public DbSettings Db { get; set; }

        public RabbitSettings Rabbit { get; set; }

        public ExchangesSettings Exchanges { get; set; }

        public string Exchange { get; set; }
    }
}
