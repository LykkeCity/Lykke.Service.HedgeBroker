﻿using System.Collections.Generic;
using Autofac;
using JetBrains.Annotations;
using Lykke.Service.HedgeBroker.Domain.Services;
using Lykke.Service.HedgeBroker.Domain.Settings;

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
            RegisterServices(builder);
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<SettingsService>()
                .As<ISettingsService>()
                .WithParameter(TypedParameter.From(_externalExchangeSettings))
                .SingleInstance();
        }
    }
}
