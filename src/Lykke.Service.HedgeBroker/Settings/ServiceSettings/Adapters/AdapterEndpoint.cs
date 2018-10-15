using JetBrains.Annotations;

namespace Lykke.Service.HedgeBroker.Settings.ServiceSettings.Adapters
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AdapterEndpoint
    {
        public string Url { get; set; }

        public string ApiKey { get; set; }
    }
}
