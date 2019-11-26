﻿using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class UpdateBalanceSteps
    {
        private BalanceUpdateModel _balanceUpdateModel;

        private BalanceViewModel _updatedBalance;

        private readonly BalanceFeatureContext _balanceFeatureContext;

        private readonly AssetFeatureContext _assetFeatureContext;

        public UpdateBalanceSteps(
            BalanceFeatureContext balanceFeatureContext,
            AssetFeatureContext assetFeatureManager)
        {
            _assetFeatureContext = assetFeatureManager;
            _balanceFeatureContext = balanceFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I set balance of asset (.*) value to (.*)")]
        public void GivenISetBalanceValueTo(string assetName, decimal value)
        {
            AssetViewModel asset = _assetFeatureContext.Assets.Values.First(a => a.Name == assetName);
            _balanceUpdateModel = new BalanceUpdateModel(asset.Id, value);
        }
        
        [When(@"I update balance for today of (.*)")]
        public async Task WhenIUpdateBalanceOfAsset(string assetName)
        {
            DateTime today = DateTime.UtcNow.Date;
            AssetViewModel asset = _assetFeatureContext.Assets.Values.First(a => a.Name == assetName);

            BalanceViewModel balance = _balanceFeatureContext.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);

            _updatedBalance = await _balanceFeatureContext.UpdateBalance(balance.Id.Value, _balanceUpdateModel);
        }
        
        [Then(@"balance amount updated to (.*)")]
        public void ThenBalanceAmountUpdatedTo(decimal amount)
        {
            _updatedBalance.Value.Should().Be(amount);
        }
    }
}
