using System.Collections.Generic;
using Autofac;
using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Core.Exchanges;
using Lykke.Service.HedgeBroker.Core.Handlers;
using Lykke.Service.HedgeBroker.Core.Services;
using Lykke.Service.HedgeBroker.Core.Settings;
using Lykke.Service.HedgeBroker.Services.Exchanges;
using Lykke.Service.HedgeBroker.Services.OrderBooks;

namespace Lykke.Service.HedgeBroker.Services
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutofacModule : Module
    {
        private readonly string _exchangeName;
        private readonly IReadOnlyList<ExternalExchangeSettings> _externalExchangeSettings;

        public AutofacModule(
            string exchangeName,
            IReadOnlyList<ExternalExchangeSettings> externalExchangeSettings)
        {
            _exchangeName = exchangeName;
            _externalExchangeSettings = externalExchangeSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterExchanges(builder);
            RegisterOrderBooks(builder);
        }

        private void RegisterExchanges(ContainerBuilder builder)
        {
            builder.RegisterType<ExternalExchange>()
                .As<IExternalExchange>()
                .SingleInstance();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeService>()
                .As<IExchangeService>()
                .WithParameter(TypedParameter.From(_exchangeName))
                .SingleInstance();

            builder.RegisterType<SettingsService>()
                .As<ISettingsService>()
                .WithParameter(TypedParameter.From(_externalExchangeSettings))
                .SingleInstance();
        }

        private void RegisterOrderBooks(ContainerBuilder builder)
        {
            builder.RegisterType<ExternalOrderBookService>()
                .As<IExternalOrderBookHandler>()
                .WithParameter(TypedParameter.From(_exchangeName))
                .SingleInstance();
        }
    }
}
