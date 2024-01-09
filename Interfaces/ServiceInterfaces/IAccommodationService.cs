using DestinationDiscoveryService.Models;

namespace DestinationDiscoveryService.Interfaces
{
    public interface IAccommodationRepository
    {
        // Define methods that interact with the database if necessary
    }

    public interface IAccommodationService
    {
        Task<IEnumerable<AccommodationModel>> SearchAccommodationsAsync(string query, decimal? budget, int? travelDistance, string userId);
        // Additional methods for other functionalities
    }
}
