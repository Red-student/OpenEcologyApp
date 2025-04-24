using System.Text.Json;
using OpenEcologyApp.Data;
using Microsoft.EntityFrameworkCore;

namespace OpenEcologyApp.Services
{
    public class JsonDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EcologyDbContext _context;

        public JsonDataService(IHttpClientFactory httpClientFactory, EcologyDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task<List<GrainHarvest>> LoadDataFromJsonAsync(string jsonUrl)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Default");
                var response = await client.GetStringAsync(jsonUrl);
                var data = JsonSerializer.Deserialize<List<GrainHarvest>>(response);
                return data ?? new List<GrainHarvest>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке JSON: {ex.Message}");
                return new List<GrainHarvest>();
            }
        }

        public async Task SaveDataToDatabaseAsync(List<GrainHarvest> data)
        {
            try
            {
                await _context.GrainHarvests.AddRangeAsync(data);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        public async Task<(List<GrainHarvest> Data, int TotalCount)> GetPagedDataAsync(int page, int pageSize, string? year = null, string? region = null)
        {
            try
            {
                var query = _context.GrainHarvests.AsQueryable();

                if (!string.IsNullOrEmpty(year))
                {
                    query = query.Where(g => g.Year.ToString() == year);
                }

                if (!string.IsNullOrEmpty(region))
                {
                    query = query.Where(g => g.Region == region);
                }

                var totalCount = await query.CountAsync();
                var data = await query
                    .OrderByDescending(g => g.Year)
                    .ThenBy(g => g.Region)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (data, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
                return (new List<GrainHarvest>(), 0);
            }
        }
    }
} 