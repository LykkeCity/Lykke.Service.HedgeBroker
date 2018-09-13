using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.HedgeBroker.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public HedgeBrokerSettings HedgeBrokerService { get; set; }
    }
}
