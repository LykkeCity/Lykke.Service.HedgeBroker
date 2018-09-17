using JetBrains.Annotations;

namespace Lykke.Service.HedgeBroker.Settings.Exchanges.External
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AdapterEndpoint
    {
        public string Url { get; set; }

        public string ApiKey { get; set; }
    }
}
