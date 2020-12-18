using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatrixPackages.Models
{
    public class Package
    {
        [Key]
        public string TrackingNumber { get; set; }
        public int ServiceTypeCode { get; set; }
        public int StatusCode { get; set; }
        public string OrderReference { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int ShipmentType { get; set; }
        public List<Item> ParcelContent { get; set; }
    }
}
