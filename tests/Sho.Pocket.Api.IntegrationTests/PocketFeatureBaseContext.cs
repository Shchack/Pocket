﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Configuration;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.DataAccess.Sql.EntityFramework;
using Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories;
using Sho.Pocket.ExchangeRates.Configuration.Models;

namespace Sho.Pocket.Api.IntegrationTests
{
    public abstract class PocketFeatureBaseContext
    {
        public ServiceProvider Services;

        public PocketFeatureBaseContext()
        {
            Configure();
        }

        public void Configure()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();

            List<ExchangeRateProviderOption> exchangeRateProviders = new List<ExchangeRateProviderOption>
            {
                new ExchangeRateProviderOption{ Name = "Default" }
            };

            services.Configure<ExchangeRateSettings>(o => { o.Providers = exchangeRateProviders; });

            services.Configure<DbSettings>(o =>
            {
                o.SystemDefaultCurrency = "USD";
            });

            ConfigureTestInMemoryDb(services);
            services.AddApplicationServices();
            services.AddMemoryCache();

            Services = services.BuildServiceProvider();

            IDbInitializer dbInitializer = Services.GetRequiredService<IDbInitializer>();
            dbInitializer.EnsureCreated();
        }

        private void ConfigureTestInMemoryDb(IServiceCollection services)
        {
            services.AddDbContext<PocketDbContext>(options => options.UseInMemoryDatabase(databaseName: "PocketDb-Test"));

            services.AddScoped<IDbInitializer, EntityFrameworkDbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<IBalanceNoteRepository, BalanceNoteRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();
        }
    }
}
