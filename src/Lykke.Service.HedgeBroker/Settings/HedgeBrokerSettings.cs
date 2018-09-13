using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HedgeBroker.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class HedgeBrokerSettings
    {
        public DbSettings Db { get; set; }
    }
}
