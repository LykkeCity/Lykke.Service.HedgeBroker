using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.HedgeBroker.Rabbit.Publishers;
using Lykke.Service.HedgeBroker.Rabbit.Subscribers;

namespace Lykke.Service.HedgeBroker.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly ExternalOrderBookSubscriber[] _externalOrderBookSubscribers;
        private readonly ExternalTickPriceSubscriber[] _externalTickPriceSubscribers;
        private readonly ExternalOrderBookPublisher _orderBookPublisher;
        private readonly ExternalTickPricePublisher _tickPricePublisher;

        public ShutdownManager(
            ExternalOrderBookSubscriber[] externalOrderBookSubscribers,
            ExternalTickPriceSubscriber[] externalTickPriceSubscribers,
            ExternalOrderBookPublisher orderBookPublisher,
            ExternalTickPricePublisher tickPricePublisher)
        {
            _externalOrderBookSubscribers = externalOrderBookSubscribers;
            _externalTickPriceSubscribers = externalTickPriceSubscribers;
            _orderBookPublisher = orderBookPublisher;
            _tickPricePublisher = tickPricePublisher;
        }

        public Task StopAsync()
        {
            foreach (ExternalOrderBookSubscriber externalOrderBookSubscriber in _externalOrderBookSubscribers)
                externalOrderBookSubscriber.Stop();

            foreach (ExternalTickPriceSubscriber externalTickPriceSubscriber in _externalTickPriceSubscribers)
                externalTickPriceSubscriber.Stop();

            _orderBookPublisher.Stop();
            _tickPricePublisher.Stop();

            return Task.CompletedTask;
        }
    }
}
