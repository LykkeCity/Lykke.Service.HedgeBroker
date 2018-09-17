using Lykke.Service.HedgeBroker.Core.Domain.OrderBooks;

namespace Lykke.Service.HedgeBroker.Core.Services
{
    public interface IExternalOrderBookPublisher : IPublisher
    {
        void Publish(OrderBook orderBook);
    }
}
