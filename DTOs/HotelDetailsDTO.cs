namespace DestinationDiscoveryService.DTOs
{
    public class HotelDetailsDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        // Include other fields based on the JSON response
        public IEnumerable<string> Amenities { get; set; }
        public IEnumerable<RoomDTO> Rooms { get; set; }
        // Add more fields as needed
    }

    public class RoomDTO
    {
        public string Type { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Facilities { get; set; }
        // Add more fields as needed for room details
    }
}
