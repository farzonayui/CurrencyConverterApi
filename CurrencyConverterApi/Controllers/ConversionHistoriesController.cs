using Microsoft.AspNetCore.Mvc;
using CurrencyConverterApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("api/conversion-history")]
    public class ConversionHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConversionHistoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var history = _context.ConvertionHistories
                .OrderByDescending(x => x.ConvertedAt)
                .ToList();

            return Ok(history);
        }
    }
}
