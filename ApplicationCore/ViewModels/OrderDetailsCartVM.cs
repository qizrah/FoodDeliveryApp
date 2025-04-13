using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.ViewModels
{
    public class OrderDetailsCartVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<ShoppingCart> ListCart { get; set; }
    }
}
