using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class UpdateCommentDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be more than 280 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be more than 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}