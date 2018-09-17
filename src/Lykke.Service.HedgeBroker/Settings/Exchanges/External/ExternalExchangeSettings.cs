using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HedgeBroker.Settings.Exchanges.External
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ExternalExchangeSettings
    {
        public string Name { get; set; }

        public decimal Fee { get; set; }

        public ExchangeSettings Rabbit { get; set; }

        [Optional]
        public AdapterEndpoint Adapter { get; set; }
    }
}
