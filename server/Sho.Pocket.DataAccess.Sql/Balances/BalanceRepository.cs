﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Balances
{
    public class BalanceRepository : BaseRepository<Balance>, IBalanceRepository
    {
        private const string SCRIPTS_DIR_NAME = "Balances.Scripts";

        public BalanceRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public List<Balance> GetAll(bool includeRelated = true)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetAllBalances.sql");

            List<Balance> result = includeRelated
                ? GetAllWithRelatedEntities(queryText)
                : base.GetAll(queryText);

            return result;
        }

        public Balance GetById(Guid id)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetBalance.sql");

            object queryParameters = new
            {
                id
            };

            Balance result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<Balance, Asset, ExchangeRate, Balance>(queryText, 
                    (balance, asset, rate) =>
                    {
                        balance.Asset = asset;
                        balance.ExchangeRate = rate;

                        return balance;
                    }, queryParameters).FirstOrDefault();
            }

            return result;
        }

        public Balance Add(Balance balance)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertBalance.sql");

            object queryParameters = new
            {
                assetId = balance.AssetId,
                effectiveDate = balance.EffectiveDate,
                value = balance.Value,
                exchangeRateId = balance.ExchangeRateId
            };

            Balance result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public void AddEffectiveBalancesTemplate(DateTime currentEffectiveDate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertBalancesTemplate.sql");

            object queryParameters = new
            {
                effectiveDate = currentEffectiveDate,
            };

            base.ExecuteScript(queryText, queryParameters);
        }

        public void Update(Balance balance)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateBalance.sql");

            object queryParameters = new
            {
                id = balance.Id,
                assetId = balance.AssetId,
                effectiveDate = balance.EffectiveDate,
                value = balance.Value,
                exchangeRateId = balance.ExchangeRateId
            };

            base.UpdateEntity(queryText, queryParameters);
        }

        public void Remove(Guid balanceId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "DeleteBalance.sql");

            object queryParameters = new
            {
                id = balanceId
            };

            base.RemoveEntity(queryText, queryParameters);
        }

        private List<Balance> GetAllWithRelatedEntities(string queryText)
        {
            List<Balance> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<Balance, Asset, ExchangeRate, Balance>(queryText,
                    (balance, asset, rate) =>
                    {
                        balance.Asset = asset;
                        balance.ExchangeRate = rate;

                        return balance;
                    }).ToList();
            }

            return result;
        }

        public IEnumerable<DateTime> GetEffectiveDates()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetBalancesEffectiveDates.sql");

            List<DateTime> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<DateTime>(queryText).ToList();
            }

            return result;
        }
    }
}
