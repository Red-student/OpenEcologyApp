using OpenEcologyApp.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace OpenEcologyApp.Services
{
    public class GrainHarvestService
    {
        private readonly EcologyDbContext _context;
        private readonly ILogger<GrainHarvestService> _logger;

        public GrainHarvestService(EcologyDbContext context, ILogger<GrainHarvestService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<GrainHarvest>> GetAllDataAsync()
        {
            return await _context.GrainHarvests.ToListAsync();
        }

        public async Task<List<GrainHarvest>> GetDataByRegionAsync(string region)
        {
            return await _context.GrainHarvests
                .Where(x => x.Region == region)
                .ToListAsync();
        }

        public async Task<List<GrainHarvest>> GetDataByYearAsync(int year)
        {
            return await _context.GrainHarvests
                .Where(x => x.Year == year)
                .ToListAsync();
        }

        public async Task<List<GrainHarvest>> GetDataByYearRangeAsync(int startYear, int endYear)
        {
            return await _context.GrainHarvests
                .Where(x => x.Year >= startYear && x.Year <= endYear)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllRegionsAsync()
        {
            return await _context.GrainHarvests
                .Select(x => x.Region)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<int>> GetAllYearsAsync()
        {
            return await _context.GrainHarvests
                .Select(x => x.Year)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }

        public async Task<string> ExportToJsonAsync()
        {
            var data = await _context.GrainHarvests.ToListAsync();
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task ImportFromJsonAsync(string jsonData)
        {
            try
            {
                var data = JsonSerializer.Deserialize<List<GrainHarvest>>(jsonData);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        // Сбрасываем ID для новых записей
                        item.Id = 0;
                        
                        // Проверяем, существует ли запись с такими же Year и Region
                        var existingItem = await _context.GrainHarvests
                            .FirstOrDefaultAsync(x => x.Year == item.Year && x.Region == item.Region);
                            
                        if (existingItem != null)
                        {
                            // Обновляем существующую запись
                            existingItem.Yield = item.Yield;
                            existingItem.SownArea = item.SownArea;
                            existingItem.GrossHarvest = item.GrossHarvest;
                        }
                        else
                        {
                            // Добавляем новую запись
                            await _context.GrainHarvests.AddAsync(item);
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при импорте данных из JSON");
                throw;
            }
        }

        public async Task InitializeDataAsync()
        {
            if (!await _context.GrainHarvests.AnyAsync())
            {
                var initialData = new List<GrainHarvest>
                {
                    // Акмолинская область
                    new GrainHarvest { Region = "Акмолинская", Year = 2004, Yield = 15.2, SownArea = 2500, GrossHarvest = 38000 },
                    new GrainHarvest { Region = "Акмолинская", Year = 2005, Yield = 15.8, SownArea = 2600, GrossHarvest = 41080 },
                    // ... остальные данные ...
                };

                await _context.GrainHarvests.AddRangeAsync(initialData);
                await _context.SaveChangesAsync();
            }
        }
    }
} 