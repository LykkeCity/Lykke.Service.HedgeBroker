using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Core.Domain.OrderBooks;
using Lykke.Service.HedgeBroker.Core.Handlers;
using Lykke.Service.HedgeBroker.Core.Services;

namespace Lykke.Service.HedgeBroker.Services.OrderBooks
{
    [UsedImplicitly]
    public class ExternalOrderBookService : IExternalOrderBookHandler
    {
        private readonly string _exchange;
        private readonly IExternalOrderBookPublisher _orderBookPublisher;

        public ExternalOrderBookService(
            string exchange,
            IExternalOrderBookPublisher orderBookPublisher)
        {
            _exchange = exchange;
            _orderBookPublisher = orderBookPublisher;
        }

        public Task HandleAsync(OrderBook externalOrderBook)
        {
            if (string.Equals(externalOrderBook.Exchange, _exchange, StringComparison.InvariantCultureIgnoreCase))
            {
                _orderBookPublisher.Publish(externalOrderBook);
            }

            return Task.CompletedTask;
        }
    }
}
