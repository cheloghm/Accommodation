using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DestinationDiscoveryService.Models
{
    public class AccommodationModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("NumberOfDays")]
        public int? NumberOfDays { get; set; }

        [BsonElement("Budget")]
        public decimal? Budget { get; set; }

        [BsonElement("DesiredLocation")]
        public string? DesiredLocation { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("TravelDistance")]
        public int? TravelDistance { get; set; }

        // Specific to Accommodations
        [BsonElement("RoomTypes")]
        public IEnumerable<string> RoomTypes { get; set; } // e.g., Single, Double, Suite

        [BsonElement("Amenities")]
        public IEnumerable<string> Amenities { get; set; } // e.g., WiFi, Pool, Gym

        [BsonElement("BookingDetails")]
        public BookingDetails BookingDetails { get; set; }

        // Other specific fields as needed
    }

    public class BookingDetails
    {
        public string BookingId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Price { get; set; }
        // Additional booking-related information
    }
}
