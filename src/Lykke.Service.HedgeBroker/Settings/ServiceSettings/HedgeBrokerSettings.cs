using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Settings.ServiceSettings.Adapters;
using Lykke.Service.HedgeBroker.Settings.ServiceSettings.Db;
using Lykke.Service.HedgeBroker.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.HedgeBroker.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class HedgeBrokerSettings
    {
        public DbSettings Db { get; set; }

        public RabbitSettings Rabbit { get; set; }

        public ExternalExchangeSettings[] Exchanges { get; set; }

        public ProxySettings Proxy { get; set; }

        public string ExchangeName { get; set; }
    }
}
