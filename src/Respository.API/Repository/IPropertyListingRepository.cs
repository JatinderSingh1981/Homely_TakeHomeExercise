using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.API;
using ViewModels.API;

namespace Repository.API
{
    public interface IPropertyListingRepository
    {
        Task<IEnumerable<PropertyListing>> GetListing(PropertyListingRequest propertyListingRequest);
        Task<long> GetListingTotal(PropertyListingRequest propertyListingRequest);
    }
}
