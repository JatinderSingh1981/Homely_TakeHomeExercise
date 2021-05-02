using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.API
{
    public enum CategoryType
    {
        Residential = 1,
        Rental,
        Land,
        Rural
    }
    public enum StatusType
    {
        Current = 1,
        Withdrawn,
        Sold,
        Leased,
        [Display(Name = "Off Market")]
        OffMarket,
        Deleted
    }

    

}
