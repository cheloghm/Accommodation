namespace DestinationDiscoveryService.Models
{
    public class CurrencyResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public List<Currency> Data { get; set; }
    }
}