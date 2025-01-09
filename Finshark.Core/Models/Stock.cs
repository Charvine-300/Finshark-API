using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Stocks")]
    public class Stock
    {
        // Primary key property
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]

        public decimal Purchase { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; }
        
        // Navigation property for related comments
        public List<Comment> Comments { get; set; } = new List<Comment>();

        // Navigation property for related portfolios
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}