using System.Threading.Tasks;
using Lykke.Service.HedgeBroker.Domain.Domain.OrderBooks;

namespace Lykke.Service.HedgeBroker.Domain.Handlers
{
    public interface IExternalOrderBookHandler
    {
        Task HandleAsync(OrderBook orderBook);
    }
}
