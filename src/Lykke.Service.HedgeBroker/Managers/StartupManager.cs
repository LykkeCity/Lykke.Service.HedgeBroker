﻿using System.Threading.Tasks;
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
        private readonly ExternalOrderBookPublisher _publisher;

        public StartupManager(
            ExternalOrderBookSubscriber[] externalOrderBookSubscribers,
            ExternalOrderBookPublisher publisher)
        {
            _externalOrderBookSubscribers = externalOrderBookSubscribers;
            _publisher = publisher;
        }

        public Task StartAsync()
        {
            _publisher.Start();

            foreach (ExternalOrderBookSubscriber externalOrderBookSubscriber in _externalOrderBookSubscribers)
                externalOrderBookSubscriber.Start();

            return Task.CompletedTask;
        }
    }
}
