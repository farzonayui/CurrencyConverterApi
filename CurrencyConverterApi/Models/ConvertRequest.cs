namespace CurrencyConverterApi.Models 
{
    public class ConvertRequest
    {
        public required string From { get; set; }
        public required string To { get; set; }
        public decimal Amount { get; set; }
    }
}


