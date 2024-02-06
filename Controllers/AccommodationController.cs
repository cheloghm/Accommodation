using DestinationDiscoveryService.Services;
using DestinationDiscoveryService.Helpers; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DestinationDiscoveryService.DTOs;

[ApiController]
[Route("[controller]")]
public class AccommodationController : ControllerBase
{
    private readonly AccommodationService _accommodationService;

    public AccommodationController(AccommodationService accommodationService)
    {
        _accommodationService = accommodationService;
    }
    
    [HttpGet("languages")]
    public async Task<IActionResult> GetLanguages()
    {
        var languages = await _accommodationService.GetLanguagesAsync();
        return Ok(languages);
    }

    [HttpGet("currencies")]
    public async Task<IActionResult> GetCurrencies()
    {
        var currencies = await _accommodationService.GetCurrenciesAsync();
        return Ok(currencies);
    }

    // Example endpoint: /Accommodation?query=<query-parameters>
    [HttpGet]
    public async Task<IActionResult> GetAccommodations(string destination, decimal budget, int days)
    {
        DateTime today = DateTime.Today;
        DateTime checkInDate = DateHelper.GetNextFriday(today);
        DateTime checkOutDate = checkInDate.AddDays(days);

        decimal minPrice = budget / 2;
        decimal maxPrice = budget;

        var destinationIds = await _accommodationService.SearchDestinationsAsync(destination);
        List<AccommodationDTO> allAccommodations = new List<AccommodationDTO>();

        foreach (var destId in destinationIds)
        {
            string query = $"dest_id={destId}&search_type=CITY&arrival_date={checkInDate:yyyy-MM-dd}&departure_date={checkOutDate:yyyy-MM-dd}&adults=1&children_age=0,17&room_qty=1&page_number=1&price_min={minPrice}&price_max={maxPrice}&languagecode=en-us&currency_code=AED";
            var accommodations = await _accommodationService.SearchAccommodationsAsync(query, budget, days);
            allAccommodations.AddRange(accommodations);
        }

        return Ok(allAccommodations);
    }

    [HttpGet("withDates")]
    public async Task<IActionResult> GetAccommodationsWithDates(string checkIn, string checkOut, decimal budget, int days)
    {
        DateTime checkInDate = DateTime.Parse(checkIn);
        DateTime checkOutDate = DateTime.Parse(checkOut);

        string query = $"arrival_date={checkInDate:yyyy-MM-dd}&departure_date={checkOutDate:yyyy-MM-dd}&adults=1"; // Modify the query as needed
        var accommodations = await _accommodationService.SearchAccommodationsAsync(query, budget, days);
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
