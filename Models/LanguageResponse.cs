namespace DestinationDiscoveryService.Models
{
    public class LanguageResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public List<Language> Data { get; set; }
    }
}