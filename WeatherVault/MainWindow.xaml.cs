using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherVault.Models;
using WeatherVault.Services;

namespace WeatherVault
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WeatherService _weatherService;
        private readonly CacheService _cacheService;
        private readonly SettingsService _settingsService;

        public MainWindow()
        {
            InitializeComponent();

            _settingsService = new SettingsService();

            // Проверяем API-ключ при запуске
            if (!_settingsService.HasApiKey())
            {
                ShowApiKeyDialog();
            }

            _weatherService = new WeatherService(_settingsService);
            _cacheService = new CacheService();

            LoadHistory();
            CityTextBox.Focus();
        }

        private void ShowApiKeyDialog()
        {
            var dialog = new ApiKeyDialog();
            if (dialog.ShowDialog() == true)
            {
                _settingsService.SetApiKey(apiKey: dialog.ApiKey);
                StatusText.Text = "✅ API-ключ сохранён!";
            }
            else
            {
                StatusText.Text = "⚠️ Без API-ключа приложение не будет работать.";
            }
        }

        private async void LoadHistory()
        {
            try
            {
                var history = await _cacheService.GetHistoryAsync();
                HistoryComboBox.Items.Clear();
                foreach (var city in history)
                {
                    HistoryComboBox.Items.Add(city);
                }
                if (history.Length > 0)
                {
                    HistoryComboBox.SelectedIndex = 0;
                }
            }
            catch
            {
                // Игнорируем ошибки загрузки истории
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var city = CityTextBox.Text.Trim();
            if (string.IsNullOrEmpty(city))
            {
                StatusText.Text = "⚠️ Введите название города";
                return;
            }

            await GetWeather(city);
        }

        private async void HistoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HistoryComboBox.SelectedItem != null)
            {
                var city = HistoryComboBox.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(city))
                {
                    CityTextBox.Text = city;
                    await GetWeather(city);
                }
            }
        }

        private void CityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var city = CityTextBox.Text.Trim();
            if (city.Length >= 2)
            {
                StatusText.Text = string.Empty;
            }
        }

        private async Task GetWeather(string city)
        {
            try
            {
                // Проверяем API-ключ
                if (!_settingsService.HasApiKey())
                {
                    ShowApiKeyDialog();
                    if (!_settingsService.HasApiKey())
                    {
                        StatusText.Text = "❌ API-ключ не введён. Используйте Настройки → Сменить API-ключ.";
                        return;
                    }
                }

                StatusText.Text = "⏳ Загрузка...";
                SearchButton.IsEnabled = false;

                // 1. Проверяем кеш
                var cached = await _cacheService.GetCachedWeatherAsync(city);
                if (cached != null)
                {
                    DisplayWeather(cached, fromCache: true);
                    StatusText.Text = $"📦 Данные из кеша (обновлено: {DateTime.Now:HH:mm})";
                    await _cacheService.AddToHistoryAsync(city);
                    LoadHistory();
                    SearchButton.IsEnabled = true;
                    return;
                }

                // 2. Запрос к API
                var weather = await _weatherService.GetWeatherAsync(city);
                if (weather == null)
                {
                    StatusText.Text = "❌ Город не найден. Проверьте название.";
                    WeatherPanel.Visibility = Visibility.Collapsed;
                    SearchButton.IsEnabled = true;
                    return;
                }

                // 3. Сохраняем в кеш
                await _cacheService.SaveToCacheAsync(city, weather);
                await _cacheService.AddToHistoryAsync(city);
                LoadHistory();

                // 4. Показываем
                DisplayWeather(weather, fromCache: false);
                StatusText.Text = $"✅ Данные из OpenWeatherMap (обновлено: {DateTime.Now:HH:mm})";
                SearchButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                StatusText.Text = $"❌ Ошибка: {ex.Message}";
                SearchButton.IsEnabled = true;
            }
        }

        private void DisplayWeather(WeatherResponse data, bool fromCache)
        {
            WeatherPanel.Visibility = Visibility.Visible;
            CityNameText.Text = data.CityName;
            TemperatureText.Text = $"{data.Main.Temperature:0}°C";

            // Эмодзи погоды (старый добрый switch без or)
            var icon = data.Weather.Count > 0 ? data.Weather[0].Icon : "";
            string emoji;

            switch (icon)
            {
                case "01d":
                case "01n":
                    emoji = "☀️";
                    break;
                case "02d":
                case "02n":
                    emoji = "⛅";
                    break;
                case "03d":
                case "03n":
                    emoji = "☁️";
                    break;
                case "04d":
                case "04n":
                    emoji = "☁️";
                    break;
                case "09d":
                case "09n":
                    emoji = "🌧️";
                    break;
                case "10d":
                case "10n":
                    emoji = "🌧️";
                    break;
                case "11d":
                case "11n":
                    emoji = "⛈️";
                    break;
                case "13d":
                case "13n":
                    emoji = "❄️";
                    break;
                case "50d":
                case "50n":
                    emoji = "🌫️";
                    break;
                default:
                    emoji = "🌤️";
                    break;
            }

            var description = data.Weather.Count > 0 ? data.Weather[0].Description : "Нет данных";
            DescriptionText.Text = $"{emoji} {description}";

            DetailsText.Text = $"💧 {data.Main.Humidity}%  |  🌬️ {data.Wind.Speed:F1} м/с  |  📊 {data.Main.Pressure} гПа";

            CacheInfoText.Text = fromCache
                ? "📦 Данные из локального кеша (до 10 минут)"
                : "🌐 Данные из OpenWeatherMap API";
        }

        private void ChangeApiKey_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ApiKeyDialog();
            if (dialog.ShowDialog() == true)
            {
                _settingsService.SetApiKey(dialog.ApiKey);
                StatusText.Text = "✅ API-ключ обновлён!";
                MessageBox.Show("✅ API-ключ успешно обновлён!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "🌤️ WeatherVault v1.0\n\n" +
                "Умный клиент погоды с локальным кешированием.\n" +
                "Данные предоставлены OpenWeatherMap API.\n\n" +
                "© 2026",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
