namespace CurrencyConverterApi.Models
{
    public class ConvertionHistory
    {
        public int Id { get; set; }
        public required string FromCurrency { get; set; }
        public required string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Result { get; set; }
        public DateTime ConvertedAt { get; set; }
    }
}
