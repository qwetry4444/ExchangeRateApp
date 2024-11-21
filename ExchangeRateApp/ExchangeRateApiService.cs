using ExchangeRateApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateApp
{
    internal class ExchangeRateApiService
    {
        HttpClient httpClient;

        public ExchangeRateApiService()
        {
            httpClient = new();
        }

        public async Task<ValCurs> GetValCurs(DateTime date)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://www.cbr-xml-daily.ru/archive/{date.Year}/{date.Month}/{date.Day}/daily_json.js");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ValCurs>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while calling the API", ex);
            }
        }
    }
}
