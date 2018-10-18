using Lykke.HttpClientGenerator;

namespace Lykke.Service.HedgeBroker.Client
{
    /// <summary>
    /// Hedge broker service client.
    /// </summary>
    public class HedgeBrokerClient : IHedgeBrokerClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HedgeBrokerClient"/> with <param name="httpClientGenerator"></param>.
        /// </summary> 
        public HedgeBrokerClient(IHttpClientGenerator httpClientGenerator)
        {
        }
    }
}
