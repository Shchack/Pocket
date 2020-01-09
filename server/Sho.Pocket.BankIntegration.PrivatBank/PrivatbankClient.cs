﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sho.BankIntegration.Privatbank
{
    public class PrivatbankClient
    {
        private const string REQUEST_MEDIA_TYPE = "text/xml";

        private readonly HttpClient _httpClient;

        public PrivatbankClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (string.IsNullOrWhiteSpace(_httpClient.BaseAddress.AbsoluteUri))
            {
                _httpClient.BaseAddress = new Uri(PrivatbankDefaultConfig.BANK_API_URL);
            }
        }

        public async Task<string> GetPublicDataAsync(string relativeUri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(relativeUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMerchantDataAsync(string relativeUri, string requestXml)
        {
            Uri requestUri = new Uri(_httpClient.BaseAddress, relativeUri);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, requestUri)
            {
                Content = new StringContent(requestXml, Encoding.UTF8, REQUEST_MEDIA_TYPE)
            };

            HttpResponseMessage response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
