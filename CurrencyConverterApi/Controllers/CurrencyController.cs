using Microsoft.AspNetCore.Mvc;
using CurrencyConverterApi.Data;
using CurrencyConverterApi.Models;
using System.Linq;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CurrencyController(AppDbContext context) => _context = context;

        [HttpPost("create")]
        public IActionResult AddRate(string code, decimal rate)
        {
            if (!Enum.TryParse<CurrencyCode>(code, true, out var currency))
                return BadRequest("Неверная валюта. Допустимо: USD, EUR, RUB");

            var existing = _context.Currency.FirstOrDefault(c => c.CurrencyCode == currency);
            if (existing != null) return BadRequest("Курс уже существует");

            var newRate = new Currency
            {
                CurrencyCode = currency,
                RateToTj = rate,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Currency.Add(newRate);
            _context.SaveChanges();

            return Ok(newRate);
        }
        [HttpPut("Update currency")]
        public IActionResult UpdateRate(CurrencyCode code, decimal rate)
        {
            var existing = _context.Currency.FirstOrDefault(c => c.CurrencyCode == code);
            
            if (existing == null)
            {
                return NotFound();
            }

            existing.RateToTj = rate;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok(existing);
        }

        [HttpGet("Get currency by code")]
        public IActionResult GetRate(CurrencyCode code)
        {
            var rate = _context.Currency.FirstOrDefault(c => c.CurrencyCode == code);
            
            if (rate == null)
            {
                return NotFound();
            }
            
            return Ok(rate);
        }

        [HttpGet("Get all currencies")]
        public IActionResult GetAllRates() => Ok(_context.Currency.ToList());

        [HttpDelete("Delete currency")]
        public IActionResult DeleteRate(CurrencyCode code)
        {
            var existing = _context.Currency.FirstOrDefault(c => c.CurrencyCode == code);
            
            if (existing == null)
            {
                return NotFound();
            }

            _context.Currency.Remove(existing);
            _context.SaveChanges();

            return Ok();
        }
    }
}