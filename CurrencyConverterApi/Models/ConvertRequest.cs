namespace CurrencyConverterApi.Models
{
    public class ConvertRequest
    {
        public CurrencyCode From { get; set; }
        public CurrencyCode To { get; set; }
        public decimal Amount { get; set; }
    }
}