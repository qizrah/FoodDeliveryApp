using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int MenuItemId { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        [ForeignKey(nameof(OrderHeaderId))]
        [JsonIgnore]
        public OrderHeader OrderHeader { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        [JsonIgnore]
        public MenuItem MenuItem { get; set; }

    }

}
