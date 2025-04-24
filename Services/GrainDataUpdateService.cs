using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenEcologyApp.Services
{
    // Класс фоновой службы, которая автоматически обновляет данные раз в сутки
    public class GrainDataUpdateService : BackgroundService
    {
        // Используется для создания области зависимостей (scope) и получения нужных сервисов
        private readonly IServiceProvider _services;

        // Интервал между обновлениями данных — 24 часа
        private readonly TimeSpan _updateInterval = TimeSpan.FromHours(24);

        // Конструктор принимает сервис-провайдер, из которого потом получаем XlsImporter
        public GrainDataUpdateService(IServiceProvider services)
        {
            _services = services;
        }

        // Метод, который вызывается при запуске фоновой службы
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Пока не получен сигнал остановки службы
            while (!stoppingToken.IsCancellationRequested)
            {
                // Создаём область зависимостей (scope) вручную, чтобы получить scoped-сервис
                using (var scope = _services.CreateScope())
                {
                    // Получаем сервис импорта XLS-файлов
                    var importer = scope.ServiceProvider.GetRequiredService<XlsImporter>();

                    // Вызываем метод импорта данных
                    await importer.ImportGrainDataAsync();

                    // Логируем в консоль факт автообновления
                    Console.WriteLine($"[Автообновление] Данные обновлены: {DateTime.Now}");
                }

                // Ждём заданное время (24 часа), прежде чем снова выполнить обновление
                await Task.Delay(_updateInterval, stoppingToken);
            }
        }
    }
}

