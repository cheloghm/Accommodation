namespace DestinationDiscoveryService.Models
{
    public class DestinationData
    {
        public List<Destination> Destinations { get; set; }
        public Pagination Pagination { get; set; }
        public AvailabilityInfo AvailabilityInfo { get; set; }
        public List<Filter> Filters { get; set; }
    }

    public class Pagination
    {
        public int NbResultsTotal { get; set; }
        // Other properties...
    }

    public class AvailabilityInfo
    {
        public int TotalAvailableNotAutoextended { get; set; }
        // Other properties...
    }

    public class Filter
    {
        public string Title { get; set; }
        // Other properties...
        public List<Option> Options { get; set; }
    }

    public class Option
    {
        public string Title { get; set; }
        // Other properties...
    }

}