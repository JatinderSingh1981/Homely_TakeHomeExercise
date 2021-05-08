using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using Models.API;
using Common.API;
using Entity = Entities.API;
using static Common.API.ExtensionMethods;

namespace Mapper.API
{
    public class PropertyListingMapper : AutoMapper.Profile
    {

        public static string MapAddress(Entity.PropertyListing listing)
        {
            return $"{listing.StreetNumber} {listing.Street}, {listing.Suburb} {listing.State} {listing.PostCode}";
        }

        public PropertyListingMapper()
        {
            #region Map from entity to model
            CreateMap<Entity.PropertyListing, PropertyListing>()
                .ForMember(m => m.CategoryType, e => e.MapFrom(o => MapEnum((CategoryType)o.CategoryType)))
                .ForMember(m => m.StatusType, e => e.MapFrom(o => MapEnum((StatusType)o.StatusType)))
                .ForMember(m => m.Address, e => e.MapFrom(o => MapAddress(o)));
            #endregion
        }

    }

}
