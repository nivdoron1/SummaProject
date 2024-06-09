using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices
{
    using System;
    using System.Collections.Generic;

    public class Invoice
    {
        public string SupplierName { get; set; }
        public string CustomerName { get; set; }
        public string SupplierId { get; set; }
        public string CustomerId { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalBeforeVAT { get; set; }
        public decimal TotalWithVAT { get; set; }
        public List<DateTime> Dates { get; set; }

        public Invoice()
        {
            Products = new List<Product>();
            Dates = new List<DateTime>();
        }
    }

}
