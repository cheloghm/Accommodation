namespace DestinationDiscoveryService.Models
{
    public class ApiProperty
    {
        public string? Name { get; set; }
        public double ReviewScore { get; set; }
        public List<string>? PhotoUrls { get; set; } = new List<string>();
        public ApiPriceBreakdown? PriceBreakdown { get; set; }
    }

}