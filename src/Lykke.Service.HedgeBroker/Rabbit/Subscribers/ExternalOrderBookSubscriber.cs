using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.HedgeBroker.Core.Consts;
using Lykke.Service.HedgeBroker.Core.Domain.OrderBooks;
using Lykke.Service.HedgeBroker.Core.Handlers;
using Lykke.Service.HedgeBroker.Rabbit.Messages.ExternalOrderBook;
using Lykke.Service.HedgeBroker.Settings.Exchanges;

namespace Lykke.Service.HedgeBroker.Rabbit.Subscribers
{
    [UsedImplicitly]
    public class ExternalOrderBookSubscriber : IDisposable
    {
        private readonly ExchangeSettings _settings;
        private readonly IExternalOrderBookHandler[] _externalOrderBookHandlers;
        private readonly string _exchangeName;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqSubscriber<ExternalOrderBook> _subscriber;

        public ExternalOrderBookSubscriber(
            ExchangeSettings settings,
            IExternalOrderBookHandler[] externalOrderBookHandlers,
            string exchangeName,
            ILogFactory logFactory)
        {
            _settings = settings;
            _externalOrderBookHandlers = externalOrderBookHandlers;
            _exchangeName = exchangeName;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            if (_exchangeName.Equals(ExchangeNames.Lykke, StringComparison.CurrentCultureIgnoreCase))
            {
                _log.Warning("Lykke exchange skipped", context: new
                {
                    Name = _exchangeName,
                    _settings.Exchange,
                    _settings.QueueSuffix
                });
                return;
            }

            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.QueueSuffix);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = false;

            _subscriber = new RabbitMqSubscriber<ExternalOrderBook>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<ExternalOrderBook>())
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

        private async Task ProcessMessageAsync(ExternalOrderBook message)
        {
            try
            {
                if (message.SellLimitOrders.Any() && message.BuyLimitOrders.Any() &&
                    message.SellLimitOrders.Min(e => e.Price) <= message.BuyLimitOrders.Max(e => e.Price))
                {
                    _log.Info("Skipped order book with negative spread", message);
                    return;
                }

                var externalOrderBook = new OrderBook
                {
                    Exchange = _exchangeName,
                    Timestamp = message.Timestamp,
                    AssetPairId = message.AssetPairId,
                    SellLimitOrders = message.SellLimitOrders.GroupBy(e => e.Price)
                        .Select(e => new OrderBookLimitOrder
                        {
                            Price = e.Key,
                            Volume = e.Sum(i => i.Volume)
                        }).ToList(),
                    BuyLimitOrders = message.BuyLimitOrders.GroupBy(e => e.Price)
                        .Select(e => new OrderBookLimitOrder
                        {
                            Price = e.Key,
                            Volume = e.Sum(i => i.Volume)
                        }).ToList()
                };

                await Task.WhenAll(_externalOrderBookHandlers.Select(o => o.HandleAsync(externalOrderBook)));
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing external order book", message);
            }
        }
    }
}
