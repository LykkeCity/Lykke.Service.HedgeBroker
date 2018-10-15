﻿using System;
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
using Lykke.Service.HedgeBroker.Settings.Exchanges;
using Lykke.Service.HedgeBroker.Settings.Exchanges.External;
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
                _settings.CurrentValue.HedgeBrokerService.Exchange,
                _settings.CurrentValue.HedgeBrokerService.Exchanges.External
                    .Where(o => o.Name != ExchangeNames.Lykke)
                    .Select(o => new Domain.Settings.ExternalExchangeSettings
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
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.Exchange))
                .SingleInstance();

            RegisterExchangeAdapters(builder);
            RegisterExchanges(builder);
            RegisterRabbit(builder);
        }

        private void RegisterExchangeAdapters(ContainerBuilder builder)
        {
            ExchangesSettings exchangesSettings = _settings.CurrentValue.HedgeBrokerService.Exchanges;

            IReadOnlyDictionary<string, Common.ExchangeAdapter.Client.AdapterEndpoint> endpoints =
                exchangesSettings.External.Where(e => e.Adapter != null).ToDictionary(o => o.Name,
                    v => new Common.ExchangeAdapter.Client.AdapterEndpoint(v.Adapter.ApiKey, new Uri(v.Adapter.Url)));

            builder.RegisterInstance(new ExchangeAdapterClientFactory(endpoints));
        }

        private void RegisterExchanges(ContainerBuilder builder)
        {
            builder.RegisterType<ExternalExchange>()
                .As<IExternalExchange>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.Exchange))
                .SingleInstance();
        }

        private void RegisterRabbit(ContainerBuilder builder)
        {
            ExchangesSettings exchangesSettings = _settings.CurrentValue.HedgeBrokerService.Exchanges;

            foreach (ExternalExchangeSettings externalExchangeSettings in exchangesSettings.External)
            {
                builder.RegisterType<ExternalOrderBookSubscriber>()
                    .AsSelf()
                    .WithParameter(TypedParameter.From(externalExchangeSettings.Name))
                    .WithParameter(TypedParameter.From(externalExchangeSettings.Rabbit))
                    .Named<ExternalOrderBookSubscriber>(externalExchangeSettings.Name)
                    .SingleInstance();
            }

            builder.RegisterType<ExternalOrderBookPublisher>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.HedgeBrokerService.Rabbit))
                .SingleInstance();
        }
    }
}
