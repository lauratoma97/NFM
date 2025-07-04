using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFM.Business.Services.Contracts
{
    public class ProductFilter
    {
        public int? Count { get; set; }
        public int? Skip { get; set; }
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
    }
}
