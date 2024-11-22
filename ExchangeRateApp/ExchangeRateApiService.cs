using ExchangeRateApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                var response = await httpClient.GetAsync($"https://www.cbr-xml-daily.ru/archive/{date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/daily_json.js");
                while (!response.IsSuccessStatusCode)
                {
                    date = date.AddDays(-1);
                    response = await httpClient.GetAsync($"https://www.cbr-xml-daily.ru/archive/{date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/daily_json.js");
                }

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
                    throw new Exception("sdf");  
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while calling the API", ex);
            }
        }
    }
}
