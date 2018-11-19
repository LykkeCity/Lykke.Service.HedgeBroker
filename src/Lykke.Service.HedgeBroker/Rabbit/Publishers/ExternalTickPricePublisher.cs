using System;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.HedgeBroker.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.HedgeBroker.Rabbit.Publishers
{
    [UsedImplicitly]
    public class ExternalTickPricePublisher : IDisposable
    {
        private readonly ILogFactory _logFactory;
        private readonly PublisherSettings _settings;
        private RabbitMqPublisher<TickPrice> _publisher;

        public ExternalTickPricePublisher(ILogFactory logFactory, PublisherSettings settings)
        {
            _logFactory = logFactory;
            _settings = settings;
        }

        public void Dispose()
        {
            _publisher?.Dispose();
        }

        public void Publish(TickPrice tickPrice)
        {
            _publisher.ProduceAsync(tickPrice);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .ForPublisher(_settings.ConnectionString, _settings.Exchange);

            _publisher = new RabbitMqPublisher<TickPrice>(_logFactory, settings)
                .SetSerializer(new JsonMessageSerializer<TickPrice>())
                .DisableInMemoryQueuePersistence()
                .Start();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }
    }
}
