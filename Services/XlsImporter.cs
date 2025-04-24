using System.Data;
using System.Net;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using OpenEcologyApp.Data;

namespace OpenEcologyApp.Services
{
    // Сервис импорта данных из XLS-файлов по урожайности, валовому сбору и посевной площади
    public class XlsImporter
    {
        private readonly EcologyDbContext _context;

        // URL-адреса XLS-файлов для разных типов данных
        private string yieldUrl = "https://stat.gov.kz/api/iblock/element/8166/file/ru/";          // Урожайность
        private string grossHarvestUrl = "https://stat.gov.kz/api/iblock/element/8160/file/ru/";   // Валовой сбор
        private string sownAreaUrl = "https://stat.gov.kz/api/iblock/element/8154/file/ru/";       // Посевная площадь

        public XlsImporter(EcologyDbContext context)
        {
            _context = context;
        }

        // Основной метод для импорта данных из всех трёх источников
        public async Task ImportGrainDataAsync()
        {
            // Регистрируем кодировки, необходимые для чтения XLS
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Считываем данные с каждого источника
            var yieldData = ReadTableFromUrl(yieldUrl);
            var grossHarvestData = ReadTableFromUrl(grossHarvestUrl);
            var sownAreaData = ReadTableFromUrl(sownAreaUrl);

            var dataList = new List<GrainHarvest>();

            // Извлекаем годы из строки заголовков (предположительно 3-я строка, индекс 2)
            var years = new List<int>();
            for (int col = 1; col < yieldData.Columns.Count; col++)
            {
                if (int.TryParse(yieldData.Rows[2][col]?.ToString(), out int year))
                    years.Add(year);
                else
                    years.Add(0); // В случае ошибки добавляем 0 (можно также выбросить исключение или логировать)
            }

            // Проходим по строкам с регионами (начиная с 2-й строки, индекс 1)
            for (int row = 1; row < 25; row++) // 25 — фиксированное число строк, можно заменить на table.Rows.Count
            {
                var region = yieldData.Rows[row][0]?.ToString();
                if (string.IsNullOrWhiteSpace(region)) continue;

                // Для каждой колонки — значения по году
                for (int col = 1; col < yieldData.Columns.Count; col++)
                {
                    var year = years[col - 1];
                    if (year == 0) continue;

                    // Пробуем распарсить значения из всех трёх таблиц
                    var yieldParsed = double.TryParse(yieldData.Rows[row][col]?.ToString(), out double yield);
                    var harvestParsed = double.TryParse(grossHarvestData.Rows[row][col]?.ToString(), out double grossHarvest);
                    var sownParsed = double.TryParse(sownAreaData.Rows[row][col]?.ToString(), out double sownArea);

                    // Если есть хотя бы одно значение — добавляем запись
                    if (yieldParsed || harvestParsed || sownParsed)
                    {
                        dataList.Add(new GrainHarvest
                        {
                            Region = region,
                            Year = year,
                            Yield = yieldParsed ? yield : 0,
                            GrossHarvest = harvestParsed ? grossHarvest : 0,
                            SownArea = sownParsed ? sownArea : 0
                        });
                    }
                }
            }

            // Обновляем базу данных: добавляем новые записи или обновляем существующие
            await UpsertGrainDataAsync(dataList);

            Console.WriteLine("Импорт всех данных завершён.");
        }

        // Метод для обновления или вставки новых записей в базу данных
        private async Task UpsertGrainDataAsync(List<GrainHarvest> newData)
        {
            foreach (var newEntry in newData)
            {
                // Проверка на наличие существующей записи по региону и году
                var existing = await _context.GrainHarvests
                    .FirstOrDefaultAsync(g => g.Region == newEntry.Region && g.Year == newEntry.Year);

                if (existing != null)
                {
                    // Обновление существующей записи
                    existing.Yield = newEntry.Yield;
                    existing.SownArea = newEntry.SownArea;
                    existing.GrossHarvest = newEntry.GrossHarvest;
                }
                else
                {
                    // Добавление новой записи
                    _context.GrainHarvests.Add(newEntry);
                }
            }

            // Сохраняем все изменения в базе данных
            await _context.SaveChangesAsync();
        }

        // Метод скачивает XLS-файл по URL и считывает его в DataTable
        private DataTable ReadTableFromUrl(string url)
        {
            // Сохраняем файл во временное хранилище
            var tempFilePath = Path.GetTempFileName();
            using (var client = new WebClient())
            {
                client.DownloadFile(url, tempFilePath);
            }

            // Чтение файла и преобразование в таблицу данных
            using var stream = File.Open(tempFilePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var dataSet = reader.AsDataSet();

            return dataSet.Tables[0]; // Возвращаем первую таблицу
        }
    }
}
