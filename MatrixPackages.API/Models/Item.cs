using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MatrixPackages.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string ItemDescription { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ItemQuantity { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ItemValue { get; set; }
    }
}
