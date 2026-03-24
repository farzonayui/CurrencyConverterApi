using Microsoft.AspNetCore.Mvc;
using CurrencyConverterApi.Data;
using CurrencyConverterApi.Models;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("api/conversions")]
    public class ConversionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConversionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Convert([FromBody] ConvertRequest req)
        {
            if (!Enum.TryParse<CurrencyCode>(req.From, true, out var from) ||
                !Enum.TryParse<CurrencyCode>(req.To, true, out var to))
                return BadRequest("Допустимые валюты: USD, EUR, RUB, TJS");

            var fromRate = _context.Currency.FirstOrDefault(c => c.CurrencyCode == from);
            var toRate = _context.Currency.FirstOrDefault(c => c.CurrencyCode == to);

            if (fromRate == null || toRate == null)
                return NotFound("Курс не найден");

            decimal result;

            if (from == CurrencyCode.TJS)
            {
                result = req.Amount / toRate.RateToTj;
            }
            else
            {
                result = req.Amount * fromRate.RateToTj;
            }

            _context.ConvertionHistories.Add(new ConvertionHistory
            {
                FromCurrency = req.From.ToUpper(),
                ToCurrency = req.To.ToUpper(),
                Amount = req.Amount,
                Result = result,
                ConvertedAt = DateTime.UtcNow
            });

            _context.SaveChanges();

            return Ok(new
            {
                from = req.From,
                to = req.To,
                amount = req.Amount,
                result = Math.Round(result, 2)
            });
        }
    }
}