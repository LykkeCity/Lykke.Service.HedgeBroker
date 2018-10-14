using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.HedgeBroker.Domain.Settings;

namespace Lykke.Service.HedgeBroker.Domain.Services
{
    public interface ISettingsService
    {
        Task<IReadOnlyList<ExternalExchangeSettings>> GetExternalExchangesAsync();
    }
}
