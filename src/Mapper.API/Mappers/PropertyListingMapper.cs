using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using Models.API;
using Common.API;
using Entity = Entities.API;

namespace Mapper.API
{
    public class PropertyListingMapper : AutoMapper.Profile
    {
        
        public static string MapCategoryEnum(int? enumValue)
        {
            if (enumValue.HasValue && enumValue.Value > 0)
                return ((CategoryType)enumValue.Value).GetDisplayName();
            return string.Empty;

        }

        public static string MapStatusEnum(int? enumValue)
        {
            if (enumValue.HasValue && enumValue.Value > 0)
                return ((StatusType)enumValue.Value).GetDisplayName();
            return string.Empty;

        }
        public static string MapAddress(Entity.PropertyListing listing)
        {
            return $"{listing.StreetNumber} {listing.Street}, {listing.Suburb} {listing.State} {listing.PostCode}";
        }

        public PropertyListingMapper()
        {

            #region Map from entity to model
            CreateMap<Entity.PropertyListing, PropertyListing>()
                .ForMember(m => m.CategoryType, e => e.MapFrom(o => MapCategoryEnum(o.CategoryType)))
                .ForMember(m => m.StatusType, e => e.MapFrom(o => MapStatusEnum(o.StatusType)))
                .ForMember(m => m.Address, e => e.MapFrom(o => MapAddress(o)));
            #endregion

           


        }

    }

}
