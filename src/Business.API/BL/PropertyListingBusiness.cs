using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.API;
using Repository.API;
using ViewModels.API;

namespace Business.API
{
    public class PropertyListingBusiness : IPropertyListingBusiness
    {
        private readonly IPropertyListingRepository _listingRepository;
        private readonly PropertyListingResponse _listingResponse;
        
        private readonly ILogger<PropertyListingBusiness> _logger;
        private readonly IMapper _mapper;

        public PropertyListingBusiness(ILogger<PropertyListingBusiness> logger, IMapper mapper, 
            IPropertyListingRepository listingRepository,
            PropertyListingResponse listingResponse)
        {
            _logger = logger;
            _mapper = mapper;
            _listingRepository = listingRepository;
            _listingResponse = listingResponse;
        }

        public async Task<PropertyListingResponse> GetListing(PropertyListingRequest request)
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[GetListing] -> {ErrorMessage}", ex.StackTrace);
            }
            return _listingResponse;
        }

      
    }
}
