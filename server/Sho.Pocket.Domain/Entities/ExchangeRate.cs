﻿using System;

namespace Sho.Pocket.Domain.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public ExchangeRate() {}

        public ExchangeRate(DateTime effectiveDate, string baseCurrency, string counterCurrency, decimal rate)
        {
            EffectiveDate = effectiveDate;
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            Rate = rate;
        }

        public Guid Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string BaseCurrency { get; set; }

        public string CounterCurrency { get; set; }

        public decimal Rate { get; set; }

        public string Provider { get; set; }
    }
}