using Microsoft.AspNetCore.Mvc;
using CurrencyConverterApi.Data;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnalyticsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("most-popular")]
        public IActionResult MostPopular()
        {
            var result = _context.ConvertionHistories
                .GroupBy(x => x.FromCurrency)
                .Select(g => new
                {
                    Currency = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            return Ok(result);
        }

        [HttpGet("weekly-stats")]
        public IActionResult Weekly()
        {
            var weekAgo = DateTime.UtcNow.AddDays(-7);

            var data = _context.ConvertionHistories
                .Where(x => x.ConvertedAt >= weekAgo)
                .ToList();

            return Ok(new
            {
                TotalOperations = data.Count,
                TopFromCurrency = data.GroupBy(x => x.FromCurrency)
                    .Select(g => new { Currency = g.Key, Count = g.Count() })
            });
        }
    }
}