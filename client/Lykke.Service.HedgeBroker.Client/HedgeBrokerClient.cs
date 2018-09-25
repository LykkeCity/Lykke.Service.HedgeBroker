using Lykke.Common.ExchangeAdapter.SpotController;
using Lykke.HttpClientGenerator;

namespace Lykke.Service.HedgeBroker.Client
{
    /// <summary>
    /// HedgeBroker API aggregating interface.
    /// </summary>
    public class HedgeBrokerClient : IHedgeBrokerClient
    {
        /// <summary>Inerface to HedgeBroker Api.</summary>
        public ISpotController SpotApi { get; private set; }

        public HedgeBrokerClient(IHttpClientGenerator httpClientGenerator)
        {
            SpotApi = httpClientGenerator.Generate<ISpotController>();
        }
    }
}
