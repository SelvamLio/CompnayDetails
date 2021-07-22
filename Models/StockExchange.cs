using System;
using System.Collections.Generic;

#nullable disable

namespace CompnayDetails.Models
{
    public partial class StockExchange
    {
        public StockExchange()
        {
            CompanyDetails = new HashSet<CompanyDetail>();
        }

        public int ExchangeId { get; set; }
        public string ExchangeName { get; set; }

        public virtual ICollection<CompanyDetail> CompanyDetails { get; set; }
    }
}
