using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Service.HedgeBroker.Rabbit.Publishers;

namespace Lykke.Service.HedgeBroker.Handlers
{
    [UsedImplicitly]
    public class ExternalOrderBookHandler 
    {
        private readonly string _exchange;
        private readonly ExternalOrderBookPublisher _orderBookPublisher;

        public ExternalOrderBookHandler(
            string exchange,
            ExternalOrderBookPublisher orderBookPublisher)
        {
            _exchange = exchange;
            _orderBookPublisher = orderBookPublisher;
        }

        public Task HandleAsync(OrderBook orderBook)
        {
            if (string.Equals(orderBook.Source, _exchange, StringComparison.InvariantCultureIgnoreCase))
            {
                _orderBookPublisher.Publish(orderBook);
            }

            return Task.CompletedTask;
        }
    }
}
