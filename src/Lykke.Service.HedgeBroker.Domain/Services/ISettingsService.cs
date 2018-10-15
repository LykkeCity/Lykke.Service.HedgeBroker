using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.HedgeBroker.Domain.Services
{
    public interface ISettingsService
    {
        Task<IReadOnlyList<ExternalExchangeSettings>> GetExternalExchangesAsync();
    }
}
