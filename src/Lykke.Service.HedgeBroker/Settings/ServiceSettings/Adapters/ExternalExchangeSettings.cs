using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Settings.ServiceSettings.Rabbit;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HedgeBroker.Settings.ServiceSettings.Adapters
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ExternalExchangeSettings
    {
        public string Name { get; set; }

        public decimal Fee { get; set; }

        public RabbitSubscribersSettings Rabbit { get; set; }

        [Optional]
        public AdapterEndpoint Adapter { get; set; }
    }
}
