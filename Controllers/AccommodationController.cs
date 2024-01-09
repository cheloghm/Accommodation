using DestinationDiscoveryService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class AccommodationController : ControllerBase
{
    private readonly AccommodationService _accommodationService;

    public AccommodationController(AccommodationService accommodationService)
    {
        _accommodationService = accommodationService;
    }

    // Example endpoint: /Accommodation?query=<query-parameters>
    [HttpGet]
    public async Task<IActionResult> GetAccommodations(string query)
    {
        var accommodations = await _accommodationService.SearchAccommodationsAsync(query);
        return Ok(accommodations);
    }

    [HttpGet("searchByCoordinates")]
    public async Task<IActionResult> SearchByCoordinates(double latitude, double longitude, string arrivalDate, string departureDate, int adults, string childrenAge, int roomQty, string languageCode = "en-us", string currencyCode = "EUR")
    {
        try
        {
            var result = await _accommodationService.SearchHotelsByCoordinatesAsync(latitude, longitude, arrivalDate, departureDate, adults, childrenAge, roomQty, languageCode, currencyCode);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("getHotelDetails")]
    public async Task<IActionResult> GetHotelDetails(int hotelId, string arrivalDate, string departureDate, int adults, string childrenAge, int roomQty, string languageCode = "en-us", string currencyCode = "EUR")
    {
        try
        {
            var hotelDetails = await _accommodationService.GetHotelDetailsAsync(hotelId, arrivalDate, departureDate, adults, childrenAge, roomQty, languageCode, currencyCode);
            return Ok(hotelDetails);
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately
            return StatusCode(500, ex.Message);
        }
    }

}
