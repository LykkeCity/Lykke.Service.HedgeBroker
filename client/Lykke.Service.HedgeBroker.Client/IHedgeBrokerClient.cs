﻿using JetBrains.Annotations;

namespace Lykke.Service.HedgeBroker.Client
{
    /// <summary>
    /// HedgeBroker client interface.
    /// </summary>
    [PublicAPI]
    public interface IHedgeBrokerClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - IHedgeBrokerApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Application Api interface</summary>
        IHedgeBrokerApi Api { get; }
    }
}
