using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.API
{
    [Table("Listings")]
    public class PropertyListing : IEntityTypeConfiguration<PropertyListing>
    {
        [Key]
        public int ListingId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string StreetNumber{ get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Street { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Suburb { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string State { get; set; }

        public int PostCode { get; set; }
        public int CategoryType { get; set; }
        public int StatusType { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string DisplayPrice { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ShortPrice { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Title { get; set; }
        
        public void Configure(EntityTypeBuilder<PropertyListing> builder)
        {
        }
    }

}
