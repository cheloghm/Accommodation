namespace DestinationDiscoveryService.DTOs
{
    public class ExchangeRateResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public ExchangeRateData Data { get; set; }
    }

    public class ExchangeRateData
    {
        public string BaseCurrency { get; set; }
        public string BaseCurrencyDate { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; }
    }

    public class ExchangeRate
    {
        public string Currency { get; set; }
        public decimal ExchangeRateBuy { get; set; }
    }

}