using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Core.Services;
using Lykke.Service.HedgeBroker.Core.Settings;

namespace Lykke.Service.HedgeBroker.Services
{
    [UsedImplicitly]
    public class SettingsService : ISettingsService
    {
        private readonly IReadOnlyList<ExternalExchangeSettings> _externalExchangeSettings;

        public SettingsService(IReadOnlyList<ExternalExchangeSettings> externalExchangeSettings)
        {
            _externalExchangeSettings = externalExchangeSettings;
        }

        public Task<IReadOnlyList<ExternalExchangeSettings>> GetExternalExchangesAsync()
        {
            return Task.FromResult(_externalExchangeSettings);
        }
    }
}
