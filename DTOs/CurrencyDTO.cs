namespace DestinationDiscoveryService.DTOs
{
    public class CurrencyDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        // Exclude EncodedSymbol if it's unnecessary for your clients
    }

}