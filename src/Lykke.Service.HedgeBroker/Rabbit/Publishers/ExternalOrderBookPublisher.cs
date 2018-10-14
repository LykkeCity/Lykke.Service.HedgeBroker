using System;
using AutoMapper;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.HedgeBroker.Domain.Domain.OrderBooks;
using Lykke.Service.HedgeBroker.Domain.Services;
using Lykke.Service.HedgeBroker.Rabbit.Messages.ExternalOrderBook;
using Lykke.Service.HedgeBroker.Settings;

namespace Lykke.Service.HedgeBroker.Rabbit.Publishers
{
    [UsedImplicitly]
    public class ExternalOrderBookPublisher : IExternalOrderBookPublisher, IDisposable
    {
        private readonly ILogFactory _logFactory;
        private readonly RabbitSettings _settings;
        private RabbitMqPublisher<ExternalOrderBook> _publisher;
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
            ExternalOrderBook externalOrderBook = Mapper.Map<ExternalOrderBook>(orderBook);

            _publisher.ProduceAsync(externalOrderBook);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForPublisher(_settings.ConnectionString, _settings.Exchange);

            _publisher = new RabbitMqPublisher<ExternalOrderBook>(_logFactory, settings)
                .SetSerializer(new JsonMessageSerializer<ExternalOrderBook>())
                .DisableInMemoryQueuePersistence()
                .Start();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }
    }
}
