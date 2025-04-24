using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenEcologyApp.Data;
using OpenEcologyApp.Services;
using Microsoft.AspNetCore.Cors;

namespace OpenEcologyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class EcologyController : ControllerBase
    {
        private readonly EcologyDbContext _context;
        private readonly JsonDataService _jsonDataService;

        public EcologyController(EcologyDbContext context, JsonDataService jsonDataService)
        {
            _context = context;
            _jsonDataService = jsonDataService;
        }

        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<ActionResult<IEnumerable<GrainHarvest>>> Get(int page, int pageSize, [FromQuery] string? year = null, [FromQuery] string? region = null)
        {
            try
            {
                var (data, totalCount) = await _jsonDataService.GetPagedDataAsync(page, pageSize, year, region);
                Response.Headers.Add("X-Total-Count", totalCount.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Ошибка при получении данных: {ex.Message}" });
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GrainHarvest>>> GetAll()
        {
            try
            {
                var data = await _context.GrainHarvests
                    .OrderByDescending(g => g.Year)
                    .ThenBy(g => g.Region)
                    .ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Ошибка при получении данных: {ex.Message}" });
            }
        }

        [HttpGet("years")]
        public async Task<ActionResult<IEnumerable<int>>> GetYears()
        {
            try
            {
                var years = await _context.GrainHarvests
                    .Select(e => e.Year)
                    .Distinct()
                    .OrderByDescending(y => y)
                    .ToListAsync();

                return Ok(years);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Ошибка при получении списка годов: {ex.Message}" });
            }
        }

        [HttpGet("regions")]
        public async Task<ActionResult<IEnumerable<string>>> GetRegions()
        {
            try
            {
                var regions = await _context.GrainHarvests
                    .Select(e => e.Region)
                    .Distinct()
                    .OrderBy(r => r)
                    .ToListAsync();

                return Ok(regions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Ошибка при получении списка регионов: {ex.Message}" });
            }
        }

        [HttpPost("load-json")]
        public async Task<IActionResult> LoadJsonData([FromBody] string jsonUrl)
        {
            try
            {
                var data = await _jsonDataService.LoadDataFromJsonAsync(jsonUrl);
                await _jsonDataService.SaveDataToDatabaseAsync(data);
                return Ok(new { message = $"Загружено {data.Count} записей" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Ошибка при загрузке данных: {ex.Message}" });
            }
        }
    }
}
