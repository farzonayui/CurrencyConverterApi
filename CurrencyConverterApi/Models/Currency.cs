namespace CurrencyConverterApi.Models
{
    public class Currency
    {
        public int Id { get; set; }

        public CurrencyCode CurrencyCode { get; set; }

        public decimal RateToTj { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}