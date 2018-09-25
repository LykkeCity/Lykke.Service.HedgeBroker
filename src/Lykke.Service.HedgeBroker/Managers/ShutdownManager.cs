using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.HedgeBroker.Core.Services;
using Lykke.Service.HedgeBroker.Rabbit.Subscribers;

namespace Lykke.Service.HedgeBroker.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly ExternalOrderBookSubscriber[] _externalOrderBookSubscribers;
        private readonly IPublisher[] _publishers;

        public ShutdownManager(
            ExternalOrderBookSubscriber[] externalOrderBookSubscribers,
            IPublisher[] publishers
        )
        {
            _externalOrderBookSubscribers = externalOrderBookSubscribers;
            _publishers = publishers;
        }

        public Task StopAsync()
        {
            foreach (ExternalOrderBookSubscriber externalOrderBookSubscriber in _externalOrderBookSubscribers)
                externalOrderBookSubscriber.Stop();

            foreach (IPublisher publisher in _publishers)
            {
                publisher.Stop();
            }

            return Task.CompletedTask;
        }
    }
}
