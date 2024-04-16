using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProductUpdated
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Design { get; set; }
        public string ImageUrl { get; set; }
    }
}
