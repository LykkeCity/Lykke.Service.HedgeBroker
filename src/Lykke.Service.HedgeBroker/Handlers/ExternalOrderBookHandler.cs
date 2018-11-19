using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Service.HedgeBroker.Rabbit.Publishers;

namespace Lykke.Service.HedgeBroker.Handlers
{
    [UsedImplicitly]
    public class ExternalOrderBookHandler 
    {
        private readonly string _exchangeName;
        private readonly ExternalOrderBookPublisher _orderBookPublisher;

        public ExternalOrderBookHandler(
            string exchangeName,
            ExternalOrderBookPublisher orderBookPublisher)
        {
            _exchangeName = exchangeName;
            _orderBookPublisher = orderBookPublisher;
        }

        public Task HandleAsync(OrderBook orderBook)
        {
            orderBook.Source = _exchangeName;

            _orderBookPublisher.Publish(orderBook);

            return Task.CompletedTask;
        }
    }
}
