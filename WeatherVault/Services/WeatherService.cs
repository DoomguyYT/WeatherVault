using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using WeatherVault.Models;

namespace WeatherVault.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "YOUR_OPENWEATHER_API_KEY"; // Замени на свой ключ!

        public WeatherService(SettingsService settingsService)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
        }

        public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            try
            {
                var url = $"weather?q={city}&appid={_apiKey}&units=metric&lang=ru";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null; // Город не найден или ошибка API
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherResponse>(json);
            }
            catch (Exception)
            {
                return null; // Ошибка сети
            }
        }

        // Прогноз на 5 дней (опционально)
        public async Task<string> GetForecastAsync(string city)
        {
            var url = $"forecast?q={city}&appid={_apiKey}&units=metric&lang=ru&cnt=5";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
