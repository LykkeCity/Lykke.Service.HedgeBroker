using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.HedgeBroker.Domain.Services;
using Lykke.Service.HedgeBroker.Rabbit.Subscribers;

namespace Lykke.Service.HedgeBroker.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly ExternalOrderBookSubscriber[] _externalOrderBookSubscribers;
        private readonly IExternalOrderBookPublisher _publisher;

        public ShutdownManager(
            ExternalOrderBookSubscriber[] externalOrderBookSubscribers,
            IExternalOrderBookPublisher publisher)
        {
            _externalOrderBookSubscribers = externalOrderBookSubscribers;
            _publisher = publisher;
        }

        public Task StopAsync()
        {
            foreach (ExternalOrderBookSubscriber externalOrderBookSubscriber in _externalOrderBookSubscribers)
                externalOrderBookSubscriber.Stop();

            _publisher.Stop();

            return Task.CompletedTask;
        }
    }
}
