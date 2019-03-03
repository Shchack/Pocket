﻿using System;
using System.Collections.Generic;
using System.IO;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.ExchangeRates.Models;

namespace Sho.Pocket.Application.Balances
{
    public interface IBalanceService
    {
        BalancesViewModel GetAll(DateTime effectiveDate);

        BalanceViewModel GetById(Guid id);

        void Add(BalanceViewModel balanceModel);

        bool AddEffectiveBalancesTemplate();

        void Update(BalanceViewModel balanceModel);

        void Delete(Guid Id);

        IEnumerable<BalanceTotalModel> GetCurrentTotalBalance();

        IEnumerable<DateTime> GetEffectiveDates();

        void ApplyExchangeRate(ExchangeRateModel model);

        IEnumerable<BalanceTotalModel> GetCurrencyTotals(Guid currencyId, int count);

        byte[] ExportBalancesToCsv();
    }
}
