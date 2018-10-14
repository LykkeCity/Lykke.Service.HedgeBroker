using Autofac;
using Common;
using Lykke.Service.HedgeBroker.Domain.Domain.OrderBooks;

namespace Lykke.Service.HedgeBroker.Domain.Services
{
    public interface IExternalOrderBookPublisher : IStartable, IStopable
    {
        void Publish(OrderBook orderBook);
    }
}
