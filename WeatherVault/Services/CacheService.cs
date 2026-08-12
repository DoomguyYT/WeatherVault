using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeatherVault.Models;

namespace WeatherVault.Services
{
    public class CacheService
    {
        private readonly WeatherContext _context;

        public CacheService()
        {
            _context = new WeatherContext();
        }

        public async Task<WeatherResponse> GetCachedWeatherAsync(string city)
        {
            try
            {
                var cached = await _context.CachedWeather
                    .FirstOrDefaultAsync(c => c.City.ToLower() == city.ToLower());

                if (cached == null)
                    return null;

                // Проверяем, не устарел ли кеш (10 минут)
                if ((DateTime.Now - cached.CachedAt).TotalMinutes > 10)
                {
                    _context.CachedWeather.Remove(cached);
                    await _context.SaveChangesAsync();
                    return null;
                }

                return JsonConvert.DeserializeObject<WeatherResponse>(cached.JsonData);
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveToCacheAsync(string city, WeatherResponse data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);

                var existing = await _context.CachedWeather
                    .FirstOrDefaultAsync(c => c.City.ToLower() == city.ToLower());

                if (existing != null)
                {
                    existing.JsonData = json;
                    existing.CachedAt = DateTime.Now;
                }
                else
                {
                    _context.CachedWeather.Add(new CachedWeather
                    {
                        City = city,
                        JsonData = json,
                        CachedAt = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                // Игнорируем ошибки кеширования
            }
        }

        public async Task AddToHistoryAsync(string city)
        {
            try
            {
                // Удаляем старые дубли
                var existing = await _context.SearchHistory
                    .Where(h => h.City.ToLower() == city.ToLower())
                    .ToListAsync();

                if (existing.Any())
                {
                    _context.SearchHistory.RemoveRange(existing);
                }

                _context.SearchHistory.Add(new SearchHistory
                {
                    City = city,
                    SearchedAt = DateTime.Now
                });

                // Оставляем только последние 5 записей
                var history = await _context.SearchHistory
                    .OrderByDescending(h => h.SearchedAt)
                    .ToListAsync();

                if (history.Count > 5)
                {
                    var toRemove = history.Skip(5);
                    _context.SearchHistory.RemoveRange(toRemove);
                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                // Игнорируем ошибки
            }
        }

        public async Task<string[]> GetHistoryAsync()
        {
            try
            {
                return await _context.SearchHistory
                    .OrderByDescending(h => h.SearchedAt)
                    .Select(h => h.City)
                    .ToArrayAsync();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }
    }
}
