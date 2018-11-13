using System;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.HedgeBroker.Handlers;
using Lykke.Service.HedgeBroker.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.HedgeBroker.Rabbit.Subscribers
{
    [UsedImplicitly]
    public class ExternalOrderBookSubscriber : IDisposable
    {
        private readonly SubscriberSettings _settings;
        private readonly ExternalOrderBookHandler _externalOrderBookHandler;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqSubscriber<OrderBook> _subscriber;

        public ExternalOrderBookSubscriber(
            SubscriberSettings settings,
            ExternalOrderBookHandler externalOrderBookHandler,
            ILogFactory logFactory)
        {
            _settings = settings;
            _externalOrderBookHandler = externalOrderBookHandler;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .ForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.QueueSuffix);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = false;

            _subscriber = new RabbitMqSubscriber<OrderBook>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<OrderBook>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        private async Task ProcessMessageAsync(OrderBook message)
        {
            try
            {
                await _externalOrderBookHandler.HandleAsync(message);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing external order book", message);
            }
        }
    }
}
