using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HedgeBroker.Client 
{
    /// <summary>
    /// HedgeBroker client settings.
    /// </summary>
    public class HedgeBrokerServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
