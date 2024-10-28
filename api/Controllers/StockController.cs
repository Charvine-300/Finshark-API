using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")] // Base URL
    [ApiController] // API Controller marker
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

 
        [HttpGet]  // GET request
        public IActionResult GetAll() {
            var stocks = _context.Stock.ToList()
            .Select(x => x.ToStockDTO()); // DTO for Stocks
            return Ok(stocks);
        }

        [HttpGet("{id}")] // GET request for details
        public IActionResult GetById([FromRoute] int id) {
            var stock = _context.Stock.Find(id);

            // Exception handling
            if (stock == null) {
                return NotFound();
            } else {
                return Ok(stock.ToStockDTO()); 
            }
                
        }

        [HttpPost] 
        public IActionResult CreateStock([FromBody] CreateStockDTO stockDTO) {
            var create = stockDTO.FromStockDTO();
            _context.Stock.Add(create);

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = create.Id }, create.ToStockDTO());
        }
    }
}