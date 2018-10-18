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
    public class ExternalTickPriceSubscriber : IDisposable
    {
        private readonly SubscriberSettings _settings;
        private readonly ExternalTickPriceHandler _externalTickPriceHandler;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqSubscriber<TickPrice> _subscriber;

        public ExternalTickPriceSubscriber(
            SubscriberSettings settings,
            ExternalTickPriceHandler externalTickPriceHandler,
            ILogFactory logFactory)
        {
            _settings = settings;
            _externalTickPriceHandler = externalTickPriceHandler;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.QueueSuffix);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = false;

            _subscriber = new RabbitMqSubscriber<TickPrice>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<TickPrice>())
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

        private async Task ProcessMessageAsync(TickPrice message)
        {
            try
            {
                await _externalTickPriceHandler.HandleAsync(message);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing external tick price", message);
            }
        }
    }
}
