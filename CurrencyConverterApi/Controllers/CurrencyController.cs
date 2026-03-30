using Microsoft.AspNetCore.Mvc;
using CurrencyConverterApi.Data;
using CurrencyConverterApi.Models;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CurrencyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult AddRate([FromBody] Currency model)
        {
            if (model.CurrencyCode == CurrencyCode.TJS && model.RateToTj != 1)
                model.RateToTj = 1;

            var existing = _context.Currency
                .FirstOrDefault(c => c.CurrencyCode == model.CurrencyCode);

            if (existing != null)
                return BadRequest("Курс уже существует");

            model.UpdatedAt = DateTime.UtcNow;

            _context.Currency.Add(model);
            _context.SaveChanges();

            return Ok(model);
        }

        [HttpPut("update")]
        public IActionResult UpdateRate(CurrencyCode code, decimal rate)
        {
            var currency = _context.Currency.FirstOrDefault(c => c.CurrencyCode == code);

            if (currency == null)
                return NotFound();

            if (code == CurrencyCode.TJS)
                rate = 1;

            currency.RateToTj = rate;
            currency.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(currency);
        }

        [HttpGet("get currency by code")]
        public IActionResult GetRate(CurrencyCode code)
        {
            var currency = _context.Currency.FirstOrDefault(c => c.CurrencyCode == code);

            if (currency == null)
                return NotFound();

            return Ok(currency);
        }

        [HttpGet("get all")]
        public IActionResult GetAll()
        {
            return Ok(_context.Currency.ToList());
        }

        [HttpDelete("delete")]
        public IActionResult Delete(CurrencyCode code)
        {
            if (code == CurrencyCode.TJS)
                return BadRequest("TJS нельзя удалить");

            var currency = _context.Currency.FirstOrDefault(c => c.CurrencyCode == code);

            if (currency == null)
                return NotFound();

            _context.Currency.Remove(currency);
            _context.SaveChanges();

            return Ok();
        }
    }
}