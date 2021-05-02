using System.Threading.Tasks;
using ViewModels.API;


namespace Business.API
{
    public interface IPropertyListingBusiness
    {
        Task<PropertyListingResponse> GetListing(PropertyListingRequest propertyListingRequest);
    }
}
