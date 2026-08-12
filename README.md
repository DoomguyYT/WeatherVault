🌤️ WeatherVault

> Умный клиент погоды с локальным кешированием в SQLite.  
> Экономь запросы к OpenWeatherMap API и всегда имей доступ к последнему прогнозу.

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![WPF](https://img.shields.io/badge/UI-WPF-purple)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![SQLite](https://img.shields.io/badge/Database-SQLite-blue)](https://www.sqlite.org/)
[![OpenWeatherMap](https://img.shields.io/badge/API-OpenWeatherMap-orange)](https://openweathermap.org/)

---

## 📖 Описание

**WeatherVault** — это десктопное приложение для просмотра прогноза погоды с **умным кешированием**. Данные загружаются из **OpenWeatherMap API** и сохраняются в локальную базу данных **SQLite** на 10 минут. Это экономит лимит API-запросов и ускоряет работу приложения.

Приложение написано на **C# с использованием WPF** и демонстрирует навыки работы с:
- HTTP-запросами и REST API
- Кешированием данных
- Entity Framework Core + SQLite
- Асинхронным программированием
- Современным UI/UX на WPF

---

## 🚀 Возможности

- 🔍 **Поиск погоды** по названию города
- 📦 **Кеширование данных** в SQLite на 10 минут
- 📋 **История последних 5 поисков**
- 🌡️ **Отображение температуры**, влажности, ветра и давления
- 🎨 **Красивый тёмный интерфейс** на WPF
- ⚡ **Асинхронные запросы** к API
- 🔑 **Безопасное хранение API-ключа** (вводится через диалог, не хранится в коде)
- 🛡️ **Обработка ошибок** — приложение не падает при проблемах с сетью

---

## 🛠️ Технологии

| Технология | Назначение |
|------------|------------|
| **C# / .NET 8** | Язык и платформа |
| **WPF** | Интерфейс |
| **Entity Framework Core** | ORM для работы с SQLite |
| **SQLite** | Локальное хранилище кеша и истории |
| **Newtonsoft.Json** | Десериализация JSON-ответов |
| **OpenWeatherMap API** | Источник данных о погоде |

---

## 📦 Установка и запуск

### 1. Клонируйте репозиторий
```bash
git clone https://github.com/yourusername/WeatherVault.git
cd WeatherVault
```

2. Получите API-ключ
Зарегистрируйтесь на OpenWeatherMap и получите бесплатный API-ключ.

3. Установите зависимости
```bash
dotnet restore
```

4. Создайте базу данных
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. Запустите приложение
```bash
dotnet run
```
При первом запуске появится окно для ввода API-ключа. Введите его и наслаждайтесь!

---
##🔮 Планы по развитию

- [ ] Прогноз на 5 дней
- [ ] Выбор единиц измерения (°C/°F)
- [ ] Автоматическое обновление каждые 10 минут
- [ ] Уведомления в трее при смене погоды
- [ ] Экспорт истории в CSV
- [ ] Поддержка нескольких городов в избранном

---

🤝 Вклад в проект
Pull Request'ы приветствуются! Если вы нашли ошибку или хотите предложить улучшение, создайте issue или отправьте PR.

---
📄 Лицензия
Этот проект распространяется под лицензией MIT. Подробнее см. в файле LICENSE.

⭐ Если проект полезен
Поставьте звёздочку на GitHub — это поможет другим разработчикам найти его.
Вопросы и предложения приветствуются в Issues.

Сделано с ❤️ и ☕

```bash
git clone https://github.com/yourusername/WeatherVault.git
cd WeatherVault
```

## ✅ **Итог**

| Что | Название |
|-----|----------|
| `Репозиторий` | `WeatherVault` |
| `Проект в Visual Studio` | `WeatherVault` |
| `Решение` | `WeatherVault.sln` |
| `Сборка` | `WeatherVault.exe` |

---


