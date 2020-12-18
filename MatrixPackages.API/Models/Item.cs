using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MatrixPackages.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyAttribute(Required = Required.AllowNull)]
        public string ItemDescription { get; set; }
        [JsonPropertyAttribute(Required = Required.Always)]
        public int ItemQuantity { get; set; }
        [JsonPropertyAttribute(Required = Required.Always)]
        public int ItemValue { get; set; }
    }
}
