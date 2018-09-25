using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.SpotController;

namespace Lykke.Service.HedgeBroker.Client
{
    /// <summary>
    /// HedgeBroker client interface.
    /// </summary>
    [PublicAPI]
    public interface IHedgeBrokerClient
    {
        /// <summary>Spot API</summary>
        ISpotController SpotApi { get; }
    }
}
