namespace DestinationDiscoveryService.Models
{
    public class DestinationResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public List<Destination> Data { get; set; }
    }
}