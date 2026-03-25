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
        public IActionResult GetMostPopularCurrensy()
        {
            var result = _context.ConvertionHistories
                .GroupBy(x => x.ToCurrency)
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
        public IActionResult GetWeeklyStats()
        {
            var weekAgo = DateTime.UtcNow.AddDays(-7);

            var weeklyOperations = _context.ConvertionHistories
                .Where(x => x.ConvertedAt >= weekAgo)
                .ToList();

            var totalOperations = weeklyOperations.Count;

            var topCurrencies = weeklyOperations
                .GroupBy(x => x.ToCurrency)
                .Select(g => new { Currency = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            return Ok(new
            {
                Period = "Last 7 days",
                TotalOperations = totalOperations,
                TopCurrencies = topCurrencies
            });
        }

        [HttpGet("rates-month")]
        public IActionResult GetRatesForMonth()
        {
            var monthAgo = DateTime.UtcNow.AddDays(-30);

            var rates = _context.Currency
                .Where(c => c.UpdatedAt >= monthAgo)
                .Select(c => new
                {
                    c.CurrencyCode,
                    c.RateToTj,
                    c.UpdatedAt
                })
                .OrderBy(c => c.UpdatedAt)
                .ToList();

            return Ok(rates);
        }
    }
}
