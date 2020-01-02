﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Configuration;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.DataAccess.Sql;
using Sho.Pocket.ExchangeRates.Configuration.Models;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public abstract class FeatureContextBase
    {
        // TODO: Remove this when UserContext will be implemented
        public readonly UserViewModel User = new UserViewModel(Guid.Parse("D6D726CD-DA50-490D-8605-A8E9125AC4B2"), "test.user@mail.com");

        protected ServiceProvider _serviceProvider;

        public FeatureContextBase()
        {
            Configure();
        }

        private void Configure()
        {
            IServiceCollection services = new ServiceCollection();

            DbSettings dbSettings = new DbSettings
            {
                DbConnectionString = ConfigurationConstants.DB_CONNECTION
            };

            services.AddSingleton(s => dbSettings);

            services.AddOptions();

            List<ExchangeRateProviderOption> exchangeRateProviders = new List<ExchangeRateProviderOption>
            {
                new ExchangeRateProviderOption{ Name = "Default" }
            };

            services.Configure<ExchangeRateSettings>(o => { o.Providers = exchangeRateProviders; });

            services.AddApplicationServices();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
