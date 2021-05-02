using System.Collections.Generic;
using Common.API;

namespace Models.API
{
    public class PropertyListing
    {
        public int ListingId { get; set; }
        public string Address { get; set; }
        public string CategoryType { get; set; }
        public string StatusType { get; set; }
        public string DisplayPrice { get; set; }
        public string Title { get; set; }
      
    }
}
