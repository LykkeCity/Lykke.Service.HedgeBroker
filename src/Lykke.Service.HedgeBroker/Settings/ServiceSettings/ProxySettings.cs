using JetBrains.Annotations;

namespace Lykke.Service.HedgeBroker.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ProxySettings
    {
        public string TargetExchange { get; set; }
    }
}
