namespace DestinationDiscoveryService.Models
{
    public class ApiProperty
    {
        public string Name { get; set; }
        public double ReviewScore { get; set; }
        public List<string> PhotoUrls { get; set; }
        public ApiPriceBreakdown PriceBreakdown { get; set; }
        // Other properties as needed
    }
}