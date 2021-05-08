using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.API;
using Repository.API;
using ViewModels.API;
using Microsoft.Extensions.Caching.Memory;
using Common.API;
using System.Text;
using System.Text.Json;

namespace Business.API
{
    public class PropertyListingBusiness : IPropertyListingBusiness
    {
        private readonly IPropertyListingRepository _listingRepository;
        private PropertyListingResponse _listingResponse;

        private readonly ILogger<PropertyListingBusiness> _logger;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cache;

        public PropertyListingBusiness(ILogger<PropertyListingBusiness> logger, IMapper mapper,
            IPropertyListingRepository listingRepository,
            PropertyListingResponse listingResponse,
            ICacheManager cache)
        {
            _logger = logger;
            _mapper = mapper;
            _listingRepository = listingRepository;
            _listingResponse = listingResponse;
            _cache = cache;
        }

        public async Task<PropertyListingResponse> GetListing(PropertyListingRequest request)
        {
            //Validate Request Parameters first
            if (request == null || ValidateRequestParameters(request) != "")
            {
                throw new ArgumentNullException(nameof(request));
            }
            try
            {
                //Generate a key from the request parameters
                var cacheKey = new StringBuilder().GenerateKey(request).ToString();

                //Get Listing from cache
                var cacheResponse = GetListingFromCache(cacheKey);

                //If listing is found in the cache, return the listing. No need to get from db
                if (cacheResponse != null && cacheResponse.Items != null)
                {
                    cacheResponse.Message = $"Cache HIT -- Found Data in Cache";
                    return cacheResponse;
                }

                //Get Listing from DB
                _logger.LogInformation($"Cache Miss -- Getting data from DB");
                _listingResponse = await GetListingFromDB(request);

                //Set Listing in the cache only if there is some result in the DB, otherwise there is no use
                if (_listingResponse.Items.Count() > 0)
                    SetListingCache(_listingResponse, cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in [GetListing] -> {ex.StackTrace}");
            }
            return _listingResponse;
        }
        private string ValidateRequestParameters(PropertyListingRequest request)
        {
            List<string> validationError = new List<string>();

            if (!request.Skip.HasValue)
            {
                validationError.Add("Specify the values you want to skip");
            }

            if (!request.Take.HasValue)
            {
                validationError.Add("Specify the values you want to Select");
            }

            if (validationError.Count > 0)
                return JsonSerializer.Serialize(validationError);
            else
                return "";
        }
        private PropertyListingResponse GetListingFromCache(string cacheKey)
        {

            var cachedResponse = _cache.GetFromCache<PropertyListingResponse>(cacheKey);
            if (cachedResponse != null && cachedResponse.Items != null)
            {
                _logger.LogInformation($"Cache HIT -- Found Data in Cache");
                return cachedResponse;
            }

            return null;
        }

        private async Task<PropertyListingResponse> GetListingFromDB(PropertyListingRequest request)
        {
            try
            {
                var listing = await _listingRepository.GetListing(request);

                var total = await _listingRepository.GetListingTotal(request);

                if (listing != null && listing.Any())
                {
                    var result = _mapper.Map<IEnumerable<PropertyListing>>(listing);
                    _listingResponse.Items = result;
                    _listingResponse.Total = total;
                    _listingResponse.Message = $"Cache Miss -- Fetched Data From DB";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[GetListing] -> {ErrorMessage}", ex.StackTrace);
            }
            return _listingResponse;
        }

        private void SetListingCache(PropertyListingResponse listingResponse, string cacheKey)
        {
            _cache.SetCache(cacheKey, _listingResponse);
        }
    }
}
