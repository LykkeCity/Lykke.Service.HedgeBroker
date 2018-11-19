using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Service.HedgeBroker.Rabbit.Publishers;

namespace Lykke.Service.HedgeBroker.Handlers
{
    [UsedImplicitly]
    public class ExternalTickPriceHandler 
    {
        private readonly string _exchangeName;
        private readonly ExternalTickPricePublisher _tickPricePublisher;

        public ExternalTickPriceHandler(
            string exchangeName,
            ExternalTickPricePublisher tickPricePublisher)
        {
            _exchangeName = exchangeName;
            _tickPricePublisher = tickPricePublisher;
        }

        public Task HandleAsync(TickPrice tickPrice)
        {
            tickPrice.Source = _exchangeName;

            _tickPricePublisher.Publish(tickPrice);

            return Task.CompletedTask;
        }
    }
}
