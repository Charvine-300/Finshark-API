using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObject queryObject) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var stocks = await _stockRepo.GetAllAsync(queryObject);
            var stockList = stocks.Select(x => x.ToStockDTO()).ToList(); // DTO for Stocks

            return Ok(stockList);
        }

        [HttpGet("{id:int}")] // GET request for details
        public async Task<IActionResult> GetById([FromRoute] int id) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var stock = await _stockRepo.GetByIdAsync(id);

            // Exception handling
            if (stock == null) {
                return NotFound("Stock does not exist");
            } else {
                return Ok(stock.ToStockDTO()); 
            }
                
        }

        [HttpPost]  // Create Requests
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO stockDTO) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var create = stockDTO.FromStockDTO();
            await _stockRepo.CreateAsync(create);

            return CreatedAtAction(nameof(GetById), new { id = create.Id }, create.ToStockDTO());
        }

        [HttpPut] 
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDTO updateDTO) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var stock = await _stockRepo.UpdateAsync(id, updateDTO);

            if (stock == null) {
                return NotFound();
            }

            return Ok(stock.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> DeleteStock([FromRoute] int id) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON
                        
            await _stockRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}