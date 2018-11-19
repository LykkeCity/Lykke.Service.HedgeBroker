using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Client;
using Lykke.Sdk;
using Lykke.Service.HedgeBroker.Domain.Consts;
using Lykke.Service.HedgeBroker.Domain.Exchanges;
using Lykke.Service.HedgeBroker.Exchanges;
using Lykke.Service.HedgeBroker.Handlers;
using Lykke.Service.HedgeBroker.Managers;
using Lykke.Service.HedgeBroker.Rabbit.Publishers;
using Lykke.Service.HedgeBroker.Rabbit.Subscribers;
using Lykke.Service.HedgeBroker.Settings;
using Lykke.Service.HedgeBroker.Settings.ServiceSettings.Adapters;
using Lykke.SettingsReader;

namespace Lykke.Service.HedgeBroker
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public AutofacModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DomainServices.AutofacModule(
                _settings.CurrentValue.HedgeBrokerService.Exchanges
                    .Where(o => o.Name != ExchangeNames.Lykke)
                    .Select(o => new Domain.ExternalExchangeSettings
                    {
                        Name = o.Name,
                        Fee = o.Fee
                    })
                    .ToList()));

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<ExternalOrderBookHandler>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.ExchangeName))
                .SingleInstance();

            builder.RegisterType<ExternalTickPriceHandler>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.ExchangeName))
                .SingleInstance();

            RegisterExchangeAdapters(builder);
            RegisterExchanges(builder);
            RegisterSubscribers(builder);
            RegisterPublishers(builder);
        }

        private void RegisterExchangeAdapters(ContainerBuilder builder)
        {
            IReadOnlyDictionary<string, Common.ExchangeAdapter.Client.AdapterEndpoint> endpoints =
                _settings.CurrentValue.HedgeBrokerService.Exchanges
                    .Where(e => e.Adapter != null)
                    .ToDictionary(
                        o => o.Name,
                        v => new Common.ExchangeAdapter.Client.AdapterEndpoint(
                            v.Adapter.ApiKey, new Uri(v.Adapter.Url)));

            builder.RegisterInstance(new ExchangeAdapterClientFactory(endpoints));
        }

        private void RegisterExchanges(ContainerBuilder builder)
        {
            builder.RegisterType<ExternalExchange>()
                .As<IExternalExchange>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.Proxy.TargetExchange))
                .SingleInstance();
        }

        private void RegisterSubscribers(ContainerBuilder builder)
        {
            string targetExchange = _settings.CurrentValue.HedgeBrokerService.Proxy.TargetExchange;

            ExternalExchangeSettings targetExchangeSettings =
                _settings.CurrentValue.HedgeBrokerService.Exchanges
                    .FirstOrDefault(o =>
                        string.Equals(o.Name, targetExchange, StringComparison.InvariantCultureIgnoreCase));

            if (targetExchangeSettings == null)
            {
                throw new InvalidOperationException("Target exchange's settings are not found.");
            }

            builder.RegisterType<ExternalOrderBookSubscriber>()
                .AsSelf()
                .WithParameter(TypedParameter.From(targetExchangeSettings.Name))
                .WithParameter(TypedParameter.From(targetExchangeSettings.Rabbit.OrderBooks))
                .Named<ExternalOrderBookSubscriber>(targetExchangeSettings.Name)
                .SingleInstance();

            builder.RegisterType<ExternalTickPriceSubscriber>()
                .AsSelf()
                .WithParameter(TypedParameter.From(targetExchangeSettings.Name))
                .WithParameter(TypedParameter.From(targetExchangeSettings.Rabbit.TickPrices))
                .Named<ExternalTickPriceSubscriber>(targetExchangeSettings.Name)
                .SingleInstance();

        }

        private void RegisterPublishers(ContainerBuilder builder)
        {
            builder.RegisterType<ExternalOrderBookPublisher>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.Rabbit.OrderBooks))
                .SingleInstance();

            builder.RegisterType<ExternalTickPricePublisher>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.Rabbit.TickPrices))
                .SingleInstance();
        }
    }
}
