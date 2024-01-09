using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DestinationDiscoveryService.DTOs;
using DestinationDiscoveryService.Models;
using Newtonsoft.Json;

namespace DestinationDiscoveryService.Services
{
    public class AccommodationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "b60759a2c7msh02abe0a3b0eb721p18b2aajsnd9514590a7fd"; // Store this securely

        public AccommodationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<AccommodationDTO>> SearchAccommodationsAsync(string query)
        {
            var client = _httpClientFactory.CreateClient();
            var requestUrl = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?{query}";

            var request = CreateHttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await client.SendAsync(request);

            return await ProcessResponse(response);
        }

        public async Task<IEnumerable<AccommodationDTO>> SearchHotelsByCoordinatesAsync(double latitude, double longitude, string arrivalDate, string departureDate, int adults, string childrenAge, int roomQty, string languageCode = "en-us", string currencyCode = "EUR")
        {
            var query = BuildCoordinatesQuery(latitude, longitude, arrivalDate, departureDate, adults, childrenAge, roomQty, languageCode, currencyCode);
            return await SearchAccommodationsAsync(query);
        }
        
        public async Task<HotelDetailsDTO> GetHotelDetailsAsync(int hotelId, string arrivalDate, string departureDate, int adults, string childrenAge, int roomQty, string languageCode = "en-us", string currencyCode = "EUR")
        {
            var query = // build your query string here;
            var requestUrl = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelDetails?{query}";

            var request = CreateHttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await _httpClientFactory.CreateClient().SendAsync(request);
            
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            // Deserialize responseBody to HotelDetailsDTO and return
            return JsonConvert.DeserializeObject<HotelDetailsDTO>(responseBody);
        }

        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string requestUrl)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    { "X-RapidAPI-Key", _apiKey },
                    { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
                },
            };

            return request;
        }

        private async Task<IEnumerable<AccommodationDTO>> ProcessResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(body);

            return MapToAccommodationDTOs(apiResponse);
        }

        private IEnumerable<AccommodationDTO> MapToAccommodationDTOs(ApiResponse apiResponse)
        {
            return apiResponse.Data.Hotels.Select(hotel => new AccommodationDTO
            {
                Name = hotel.Property.Name,
                Address = hotel.AccessibilityLabel.Split('\n').Last(),
                Rating = hotel.Property.ReviewScore,
                PhotoUrls = hotel.Property.PhotoUrls,
                Price = hotel.Property.PriceBreakdown.GrossPrice.Value,
                Description = hotel.AccessibilityLabel
                // Map other fields as necessary
            }).ToList();
        }

        private string BuildCoordinatesQuery(double latitude, double longitude, string arrivalDate, string departureDate, int adults, string childrenAge, int roomQty, string languageCode, string currencyCode)
        {
            // Build the query string for search by coordinates
            // Include latitude, longitude, and other parameters as needed
            return $"latitude={latitude}&longitude={longitude}&arrivalDate={arrivalDate}&departureDate={departureDate}&adults={adults}&childrenAge={childrenAge}&roomQty={roomQty}&languageCode={languageCode}&currencyCode={currencyCode}";
        }
    }
}
