using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherVault.Models
{
    public class WeatherResponse
    {
        [JsonProperty("name")]
        public string CityName { get; set; }

        [JsonProperty("main")]
        public MainData Main { get; set; }

        [JsonProperty("weather")]
        public List<WeatherDescription> Weather { get; set; }

        [JsonProperty("wind")]
        public WindData Wind { get; set; }

        [JsonProperty("dt")]
        public long Timestamp { get; set; }
    }

    public class MainData
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }
    }

    public class WeatherDescription
    {
        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public class WindData
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
    }

    // Модель для кеша в SQLite
    public class CachedWeather
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string JsonData { get; set; }  // Храним весь ответ как JSON
        public DateTime CachedAt { get; set; }
    }

    // Модель для истории поиска
    public class SearchHistory
    {
        public int Id { get; set; }
        public string City { get; set; }
        public DateTime SearchedAt { get; set; }
    }
}
