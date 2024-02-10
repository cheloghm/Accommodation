namespace DestinationDiscoveryService.DTOs
{
    public class LocationToLatLongResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public List<LocationData> Data { get; set; }
    }

    public class LocationData
    {
        public string Name { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
