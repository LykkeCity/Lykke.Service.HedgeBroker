using System.Threading.Tasks;
using Lykke.Service.HedgeBroker.Core.Domain.OrderBooks;

namespace Lykke.Service.HedgeBroker.Core.Handlers
{
    public interface IExternalOrderBookHandler
    {
        Task HandleAsync(OrderBook orderBook);
    }
}
