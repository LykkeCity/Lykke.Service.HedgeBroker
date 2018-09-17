using System;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.HedgeBroker.Core.Domain.OrderBooks;
using Lykke.Service.HedgeBroker.Core.Services;
using Lykke.Service.HedgeBroker.Settings;

namespace Lykke.Service.HedgeBroker.Rabbit.Publishers
{
    [UsedImplicitly]
    public class ExternalOrderBookPublisher : IExternalOrderBookPublisher, IDisposable
    {
        private readonly ILogFactory _logFactory;
        private readonly RabbitSettings _settings;
        private RabbitMqPublisher<OrderBook> _publisher;
        private readonly ILog _log;

        public ExternalOrderBookPublisher(ILogFactory logFactory, RabbitSettings settings)
        {
            _logFactory = logFactory;
            _settings = settings;
            _log = logFactory.CreateLog(this);
        }

        public void Dispose()
        {
            _publisher?.Dispose();
        }

        public void Publish(OrderBook orderBook)
        {
            _publisher.ProduceAsync(orderBook);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForPublisher(_settings.ConnectionString, _settings.Exchange);

            _publisher = new RabbitMqPublisher<OrderBook>(_logFactory, settings)
                .SetSerializer(new JsonMessageSerializer<OrderBook>())
                .DisableInMemoryQueuePersistence()
                .Start();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }
    }
}
