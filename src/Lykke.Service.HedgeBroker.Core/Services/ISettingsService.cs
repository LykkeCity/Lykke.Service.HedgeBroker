using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.HedgeBroker.Core.Settings;

namespace Lykke.Service.HedgeBroker.Core.Services
{
    public interface ISettingsService
    {
        Task<IReadOnlyList<ExternalExchangeSettings>> GetExternalExchangesAsync();
    }
}
