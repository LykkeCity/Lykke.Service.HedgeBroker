using JetBrains.Annotations;

namespace Lykke.Service.HedgeBroker.Settings.ServiceSettings.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitPublishersSettings
    {
        public PublisherSettings OrderBooks { get; set; }

        public PublisherSettings TickPrices { get; set; }
    }
}
