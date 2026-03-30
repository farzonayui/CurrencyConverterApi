using Microsoft.AspNetCore.Mvc;
using CurrencyConverterApi.Data;
using CurrencyConverterApi.Models;
using CurrencyConverterApi.Services;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("api/conversions")]
    public class ConversionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ConversionService _service;

        public ConversionController(AppDbContext context)
        {
            _context = context;
            _service = new ConversionService(context);
        }

        [HttpPost]
        public IActionResult Convert([FromBody] ConvertRequest req)
        {
            var result = _service.Convert(req.Amount, req.From, req.To);

            _context.ConvertionHistories.Add(new ConvertionHistory
            {
                FromCurrency = req.From.ToString(),
                ToCurrency = req.To.ToString(),
                Amount = req.Amount,
                Result = result,
                ConvertedAt = DateTime.UtcNow
            });

            _context.SaveChanges();

            return Ok(new
            {
                req.From,
                req.To,
                req.Amount,
                result = Math.Round(result, 2)
            });
        }
    }
}