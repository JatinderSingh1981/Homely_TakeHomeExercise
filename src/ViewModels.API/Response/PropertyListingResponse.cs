using Models.API;
using System.Collections.Generic;

namespace ViewModels.API
{
    public class PropertyListingResponse
    {
        public IEnumerable<PropertyListing> Items { get; set; }
        public long Total { get; set; }
        
    }
}
