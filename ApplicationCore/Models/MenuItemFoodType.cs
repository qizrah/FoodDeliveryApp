using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class MenuItemFoodType
    {
        [Key]
        public int mfid { get; set; }

        public int MenuItemId { get; set; }
        [JsonIgnore] // Prevents cycle
        public virtual MenuItem MenuItem { get; set; }

        public int FoodTypeId { get; set; }
        public virtual FoodType FoodType { get; set; }
    }
}
