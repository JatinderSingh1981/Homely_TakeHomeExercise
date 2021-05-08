using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Common.API;
using Entities.API;
using DBContext.API;
using ViewModels.API;

namespace Repository.API
{
    public class PropertyListingRepository : IPropertyListingRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<PropertyListingRepository> _logger;

        public PropertyListingRepository(DataContext context, IOptions<AppSettings> settings,
            ILogger<PropertyListingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PropertyListing>> GetListing(PropertyListingRequest request)
        {
            _logger.LogDebug("[GetListing] -> Getting Property list from DB");

            return await _context.PropertyListing
                .Where(x => String.IsNullOrEmpty(request.Suburb) || x.Suburb == request.Suburb)
                .Where(x => request.CategoryType == null || x.CategoryType == (int)request.CategoryType.Value)
                .Where(x => request.StatusType == null || x.StatusType == (int)request.StatusType.Value)
                .OrderBy(x => x.ListingId) //Default Ordering
                .Skip(request.Skip.Value).Take(request.Take.Value).ToListAsync();
        }

        public async Task<long> GetListingTotal(PropertyListingRequest request)
        {
            _logger.LogDebug("[GetListing] -> Getting Property list Count from DB");

            return await _context.PropertyListing
                .Where(x => String.IsNullOrEmpty(request.Suburb) || x.Suburb == request.Suburb)
                .Where(x => request.CategoryType == null || x.CategoryType == (int)request.CategoryType.Value)
                .Where(x => request.StatusType == null || x.StatusType == (int)request.StatusType.Value)
                .LongCountAsync();
        }
    }
}
