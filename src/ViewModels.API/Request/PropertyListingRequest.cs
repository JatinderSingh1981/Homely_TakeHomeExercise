using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.API;

namespace ViewModels.API
{
    public class PropertyListingRequest
    {
        public string Suburb { get; set; }
        public CategoryType? CategoryType { get; set; }
        public StatusType? StatusType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public int? Skip { get; set; } = 0;

        [Range(5, 500, ErrorMessage = "Value must be between 0 and 500")]
        public int? Take { get; set; } = 10;

    }
}
