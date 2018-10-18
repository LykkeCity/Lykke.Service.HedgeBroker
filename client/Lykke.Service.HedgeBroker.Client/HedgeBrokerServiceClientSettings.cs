using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HedgeBroker.Client
{
    /// <summary>
    /// Settings for Hedge broker service client.
    /// </summary>
    [PublicAPI]
    public class HedgeBrokerServiceClientSettings
    {
        /// <summary>
        /// Service url.
        /// </summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
