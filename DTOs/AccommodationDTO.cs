namespace DestinationDiscoveryService.DTOs
{
    public class AccommodationDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public IEnumerable<string> PhotoUrls { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        // Add more fields as per your requirement

        public decimal DailyRate { get; set; }
        public decimal LeftoverBudget { get; set; }
        public int AdditionalDaysPossible { get; set; }
    }
}
