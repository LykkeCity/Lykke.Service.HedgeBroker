using JetBrains.Annotations;

namespace Lykke.Service.HedgeBroker.Settings.ServiceSettings.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSubscribersSettings
    {
        public SubscriberSettings OrderBooks { get; set; }

        public SubscriberSettings TickPrices { get; set; }
    }
}
