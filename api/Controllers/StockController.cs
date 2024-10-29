using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetAll() {
            var stocks = await _context.Stock.ToListAsync();

            var stockList = stocks.Select(x => x.ToStockDTO()); // DTO for Stocks
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

        [HttpPost]  // Create Requests
        public IActionResult CreateStock([FromBody] CreateStockDTO stockDTO) {
            var create = stockDTO.FromStockDTO();
            _context.Stock.Add(create);

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = create.Id }, create.ToStockDTO());
        }

        [HttpPut] 
        [Route("{id}")]
        public IActionResult UpdateStock([FromRoute] int id, [FromBody] UpdateStockDTO updateDTO) {
            var stock = _context.Stock.FirstOrDefault(x => x.Id == id);

            if (stock == null) {
                return NotFound();
            }

            // Updating record
            stock.Symbol = updateDTO.Symbol;
            stock.CompanyName = updateDTO.CompanyName;
            stock.Industry = updateDTO.Industry;
            stock.LastDiv = updateDTO.LastDiv;
            stock.Purchase = updateDTO.Purchase;
            stock.MarketCap = updateDTO.MarketCap;

            _context.SaveChanges();

            return Ok(stock.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]

        public IActionResult DeleteStock([FromRoute] int id) {
            var stock = _context.Stock.FirstOrDefault(x => x.Id == id);

            if (stock == null) {
                return NotFound();
            }

            _context.Stock.Remove(stock);

            _context.SaveChanges();

            return NoContent();
        }
    }
}