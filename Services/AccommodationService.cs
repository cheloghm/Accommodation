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
        private readonly string _baseApiUrl = "https://booking-com15.p.rapidapi.com";

        public AccommodationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private string BuildRequestUrl(string endpoint, string queryParams)
        {
            return $"{_baseApiUrl}/api/v1/hotels/{endpoint}?{queryParams}";
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "X-RapidAPI-Key", _apiKey },
                    { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
                },
            };

            return await client.SendAsync(request);
        }

        public async Task<IEnumerable<LanguageDTO>> GetLanguagesAsync()
        {
            var url = BuildRequestUrl("meta/getLanguages", "");
            var response = await SendHttpRequestAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<LanguageDTO>();
            }

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LanguageResponse>(body);
            return result.Data.Select(l => new LanguageDTO
            {
                Name = l.Name,
                Code = l.Code
            });
        }

        public async Task<IEnumerable<CurrencyDTO>> GetCurrenciesAsync()
        {
            var url = BuildRequestUrl("meta/getCurrency", "");
            var response = await SendHttpRequestAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<CurrencyDTO>();
            }

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CurrencyResponse>(body);
            return result.Data.Select(c => new CurrencyDTO
            {
                Name = c.Name,
                Code = c.Code,
                Symbol = c.Symbol
            });
        }

        public async Task<ExchangeRateResponse> GetExchangeRatesAsync(string baseCurrency = "USD")
        {
            var url = BuildRequestUrl("meta/getExchangeRates", $"base_currency={baseCurrency}");
            var response = await SendHttpRequestAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching exchange rates: {response.ReasonPhrase}");
            }

            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeRateResponse>(body);
        }

        public async Task<IEnumerable<string>> SearchDestinationsAsync(string query)
        {
            var url = BuildRequestUrl("searchDestination", $"query={query}");
            var response = await SendHttpRequestAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<string>();
            }

            var body = await response.Content.ReadAsStringAsync();
            var destinationResponse = JsonConvert.DeserializeObject<DestinationResponse>(body);
            return destinationResponse.Data.Select(d => d.Dest_id); // Access Dest_id directly
        }

        public async Task<IEnumerable<AccommodationDTO>> SearchAccommodationsAsync(string query, decimal? totalBudget = null, int? numberOfDays = null)
        {
            var url = BuildRequestUrl("searchHotels", $"query={query}");
            var response = await SendHttpRequestAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error, log it or throw an exception
                return Enumerable.Empty<AccommodationDTO>();
            }

            var body = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(body);

            decimal? dailyBudget = null;
            if (totalBudget.HasValue && numberOfDays.HasValue && numberOfDays.Value > 0)
            {
                dailyBudget = CalculateDailyBudget(totalBudget.Value, numberOfDays.Value);
            }

            return MapToAccommodationDTOs(apiResponse, dailyBudget, numberOfDays);
        }

        public async Task<HotelDetailsDTO> GetHotelDetailsAsync(int hotelId, string arrivalDate, string departureDate, int adults, string childrenAge, int roomQty, string languageCode = "en-us", string currencyCode = "EUR")
        {
            var query = "";// build your query string here;
            var url = BuildRequestUrl("getHotelDetails", $"query={query}");
            var response = await SendHttpRequestAsync(url);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HotelDetailsDTO>(responseBody);
        }

        public decimal CalculateDailyBudget(decimal totalBudget, int numberOfDays)
        {
            return Math.Ceiling(totalBudget / numberOfDays);
        }

        private async Task<IEnumerable<AccommodationDTO>> ProcessResponse(HttpResponseMessage response, decimal? dailyBudget = null, int? numberOfDays = null)
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(body);

            return MapToAccommodationDTOs(apiResponse, dailyBudget, numberOfDays);
        }

        private IEnumerable<AccommodationDTO> MapToAccommodationDTOs(ApiResponse apiResponse, decimal? dailyBudget, int? numberOfDays)
        {
            if (apiResponse?.Data?.Hotels == null || numberOfDays == null)
            {
                return Enumerable.Empty<AccommodationDTO>();
            }

            int days = numberOfDays.Value;
            decimal minBudget = dailyBudget.HasValue ? dailyBudget.Value / 2 : 0;
            decimal maxBudget = dailyBudget ?? decimal.MaxValue;

            return apiResponse.Data.Hotels.Select(hotel =>
            {
                decimal grossPrice = hotel.Property.PriceBreakdown.GrossPrice.Value;
                decimal dailyRate = grossPrice / days;

                if (dailyRate < minBudget || dailyRate > maxBudget)
                {
                    return null; // Skip hotels outside the budget range
                }

                decimal leftoverBudget = dailyBudget.Value - dailyRate;
                int additionalDaysPossible = (int)(leftoverBudget / dailyRate);

                return new AccommodationDTO
                {
                    Name = hotel.Property.Name,
                    Address = hotel.AccessibilityLabel.Split('\n').LastOrDefault(),
                    Rating = hotel.Property.ReviewScore,
                    PhotoUrls = hotel.Property.PhotoUrls ?? new List<string>(),
                    Price = grossPrice,
                    DailyRate = dailyRate,
                    LeftoverBudget = leftoverBudget,
                    AdditionalDaysPossible = additionalDaysPossible,
                    Description = hotel.AccessibilityLabel
                };
            })
            .Where(dto => dto.DailyRate <= (dailyBudget ?? dto.DailyRate))
            .ToList();
        }

    }
}
