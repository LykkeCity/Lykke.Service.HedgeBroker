using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Domain.Services;
using Lykke.Service.HedgeBroker.Domain.Settings;

namespace Lykke.Service.HedgeBroker.DomainServices
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
