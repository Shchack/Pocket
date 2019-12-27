﻿using Newtonsoft.Json;
using Sho.Pocket.BankIntegration.Monobank.Abstractions;
using Sho.Pocket.BankIntegration.Monobank.Common;
using Sho.Pocket.BankIntegration.Monobank.Models;
using Sho.Pocket.Core.BankIntegration.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sho.Pocket.BankIntegration.Monobank
{
    public class MonobankAccountService : IMonobankAccountService
    {
        private const string _bankName = "Monobank";

        private readonly string _bankApiUrl = "https://api.monobank.ua/";

        public async Task<List<BankAccountBalance>> GetClientAccountsInfoAsync(BankClientData clientData)
        {
            string requestUri = _bankApiUrl + "personal/client-info";
            List<BankAccountBalance> result;

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Add("X-Token", clientData.Token);

                HttpResponseMessage response = await client.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();

                result = ParseClientAccountsInfo(content);
            }

            return result;
        }

        public Task<string> GetClientAccountExctractAsync(string token)
        {
            throw new NotImplementedException();
        }

        private List<BankAccountBalance> ParseClientAccountsInfo(string content)
        {
            MonobankUserInfo userInfo = JsonConvert.DeserializeObject<MonobankUserInfo>(content);
            var currencyCodeConverter = new ISO4217CurrencyCodeConverter();
            List<BankAccountBalance> result = new List<BankAccountBalance>();

            foreach (MonobankAccount account in userInfo.Accounts)
            {
                decimal balance = (decimal)account.Balance / 100;
                string currency = currencyCodeConverter.GetCurrencyName(account.CurrencyCode.ToString());
                string name = GetFriendlyAccountName(balance, currency);

                result.Add(new BankAccountBalance(_bankName, account.Id, name, currency, balance));
            }

            return result;
        }

        private string GetFriendlyAccountName(decimal balance, string currency)
        {
            return $"{_bankName}: {balance} {currency}";
        }
    }
}