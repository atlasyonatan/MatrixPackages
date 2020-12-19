using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MatrixPackages.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public string ItemDescription { get; set; }

        [Required]
        public int ItemQuantity { get; set; }

        [Required]
        public int ItemValue { get; set; }
    }
}
