using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")] // Base URL
    [ApiController] // API Controller marker
    public class StockController : ControllerBase
    {

        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

 
        [HttpGet]  // GET request
        public async Task<IActionResult> GetAllAsync() {
            var stocks = await _stockRepo.GetAllAsync();
            var stockList = stocks.Select(x => x.ToStockDTO()); // DTO for Stocks

            return Ok(stockList);
        }

        [HttpGet("{id}")] // GET request for details
        public async Task<IActionResult> GetById([FromRoute] int id) {
            var stock = await _stockRepo.GetByIdAsync(id);

            // Exception handling
            if (stock == null) {
                return NotFound();
            } else {
                return Ok(stock.ToStockDTO()); 
            }
                
        }

        [HttpPost]  // Create Requests
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO stockDTO) {
            var create = stockDTO.FromStockDTO();
            await _stockRepo.CreateAsync(create);

            return CreatedAtAction(nameof(GetById), new { id = create.Id }, create.ToStockDTO());
        }

        [HttpPut] 
        [Route("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDTO updateDTO) {
            var stock = await _stockRepo.UpdateAsync(id, updateDTO);

            if (stock == null) {
                return NotFound();
            }

            return Ok(stock.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteStock([FromRoute] int id) {
            await _stockRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}