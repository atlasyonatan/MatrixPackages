using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MatrixPackages.Models
{
    public class Package
    {
        [Key]
        [Required(AllowEmptyStrings = false)]
        public string TrackingNumber { get; set; }

        [Required]
        public int ServiceTypeCode { get; set; }

        [Required]
        public int StatusCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string OrderReference { get; set; }

        [Required]
        public DateTime ShipmentDate { get; set; }

        [Required]
        public int ShipmentType { get; set; }

        [Required]
        public List<Item> ParcelContent { get; set; }
    }
}
