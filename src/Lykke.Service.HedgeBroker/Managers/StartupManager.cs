using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.HedgeBroker.Core.Services;
using Lykke.Service.HedgeBroker.Rabbit.Subscribers;

namespace Lykke.Service.HedgeBroker.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly ExternalOrderBookSubscriber[] _externalOrderBookSubscribers;
        private readonly IPublisher[] _publishers;

        public StartupManager(
            ExternalOrderBookSubscriber[] externalOrderBookSubscribers,
            IPublisher[] publishers)
        {
            _externalOrderBookSubscribers = externalOrderBookSubscribers;
            _publishers = publishers;
        }

        public Task StartAsync()
        {
            foreach (ExternalOrderBookSubscriber externalOrderBookSubscriber in _externalOrderBookSubscribers)
                externalOrderBookSubscriber.Start();

            foreach (IPublisher publisher in _publishers)
            {
                publisher.Start();
            }

            return Task.CompletedTask;
        }
    }
}
