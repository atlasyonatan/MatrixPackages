using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MatrixPackages.Models
{
    public class Package
    {
        [Key]
        [JsonProperty(Required = Required.Always)]
        public string TrackingNumber { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ServiceTypeCode { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int StatusCode { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string OrderReference { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime ShipmentDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ShipmentType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public List<Item> ParcelContent { get; set; }
    }
}
