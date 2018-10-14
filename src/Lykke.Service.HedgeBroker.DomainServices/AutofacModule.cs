using System.Collections.Generic;
using Autofac;
using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Domain.Exchanges;
using Lykke.Service.HedgeBroker.Domain.Handlers;
using Lykke.Service.HedgeBroker.Domain.Services;
using Lykke.Service.HedgeBroker.Domain.Settings;
using Lykke.Service.HedgeBroker.DomainServices.Exchanges;
using Lykke.Service.HedgeBroker.DomainServices.OrderBooks;

namespace Lykke.Service.HedgeBroker.DomainServices
{
    [UsedImplicitly]
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
            RegisterServices(builder);
        }

        private void RegisterExchanges(ContainerBuilder builder)
        {
            builder.RegisterType<ExternalExchange>()
                .As<IExternalExchange>()
                .WithParameter(TypedParameter.From(_exchangeName))
                .SingleInstance();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
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
