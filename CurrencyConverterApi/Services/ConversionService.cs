using CurrencyConverterApi.Data;
using CurrencyConverterApi.Models;

namespace CurrencyConverterApi.Services
{
    public class ConversionService
    {
        private readonly AppDbContext _context;

        public ConversionService(AppDbContext context)
        {
            _context = context;
        }

        public decimal Convert(decimal amount, CurrencyCode from, CurrencyCode to)
        {
            var fromRate = _context.Currency.First(c => c.CurrencyCode == from).RateToTj;
            var toRate = _context.Currency.First(c => c.CurrencyCode == to).RateToTj;

            decimal inTjs = from == CurrencyCode.TJS
                ? amount
                : amount * fromRate;

            return to == CurrencyCode.TJS
                ? inTjs
                : inTjs / toRate;
        }
    }
}