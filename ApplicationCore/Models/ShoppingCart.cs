using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int MenuItemId { get; set; }
        public int Count { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        [JsonIgnore]
        public virtual MenuItem MenuItem { get; set; }

    }
}
