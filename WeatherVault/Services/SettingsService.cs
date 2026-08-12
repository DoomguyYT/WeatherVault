using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace WeatherVault.Services
{
    public class SettingsService
    {
        private readonly string _settingsPath = "settings.json";
        private Settings _settings;

        public SettingsService()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (File.Exists(_settingsPath))
            {
                try
                {
                    var json = File.ReadAllText(_settingsPath);
                    _settings = System.Text.Json.JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
                catch
                {
                    _settings = new Settings();
                }
            }
            else
            {
                _settings = new Settings();
            }
        }

        public void SaveSettings()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_settingsPath, json);
        }

        public string GetApiKey()
        {
            return _settings.ApiKey ?? string.Empty;
        }

        public void SetApiKey(string apiKey)
        {
            _settings.ApiKey = apiKey;
            SaveSettings();
        }

        public bool HasApiKey()
        {
            return !string.IsNullOrEmpty(_settings.ApiKey);
        }

        private class Settings
        {
            public string ApiKey { get; set; } = string.Empty;
        }
    }
}
