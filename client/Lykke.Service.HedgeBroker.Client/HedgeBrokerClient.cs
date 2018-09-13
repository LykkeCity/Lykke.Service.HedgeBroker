using Lykke.HttpClientGenerator;

namespace Lykke.Service.HedgeBroker.Client
{
    /// <summary>
    /// HedgeBroker API aggregating interface.
    /// </summary>
    public class HedgeBrokerClient : IHedgeBrokerClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to HedgeBroker Api.</summary>
        public IHedgeBrokerApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public HedgeBrokerClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IHedgeBrokerApi>();
        }
    }
}
