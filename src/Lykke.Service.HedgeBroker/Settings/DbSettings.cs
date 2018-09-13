using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HedgeBroker.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnectionString { get; set; }
    }
}
