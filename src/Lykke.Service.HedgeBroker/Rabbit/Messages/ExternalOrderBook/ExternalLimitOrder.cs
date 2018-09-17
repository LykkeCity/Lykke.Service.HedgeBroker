using JetBrains.Annotations;

namespace Lykke.Service.HedgeBroker.Rabbit.Messages.ExternalOrderBook
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ExternalLimitOrder
    {
        public decimal Price { get; set; }

        public decimal Volume { get; set; }
    }
}
