using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class CreateStockDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Symbol cannot be empty")]
        [MaxLength(10, ErrorMessage = "Stock symbol cannot be over 10 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Company name cannot be empty")]
        [MaxLength(10, ErrorMessage = "Company name cannot be over 10 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

[Required]
[MaxLength(30, ErrorMessage = "Industry cannot be over 10")]
        public string Industry { get; set; } = string.Empty;

[Required]
[Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}