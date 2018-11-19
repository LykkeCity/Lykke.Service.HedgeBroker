using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.HedgeBroker.Rabbit.Publishers;
using Lykke.Service.HedgeBroker.Rabbit.Subscribers;

namespace Lykke.Service.HedgeBroker.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly ExternalOrderBookSubscriber[] _externalOrderBookSubscribers;
        private readonly ExternalTickPriceSubscriber[] _externalTickPriceSubscribers;
        private readonly ExternalOrderBookPublisher _orderBookPublisher;
        private readonly ExternalTickPricePublisher _tickPricePublisher;

        public StartupManager(
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

        public Task StartAsync()
        {
            _orderBookPublisher.Start();
            _tickPricePublisher.Start();

            foreach (ExternalOrderBookSubscriber externalOrderBookSubscriber in _externalOrderBookSubscribers)
                externalOrderBookSubscriber.Start();

            foreach (ExternalTickPriceSubscriber externalTickPriceSubscriber in _externalTickPriceSubscribers)
                externalTickPriceSubscriber.Start();

            return Task.CompletedTask;
        }
    }
}
