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
using Finshark.Core.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")] // Base URL
    [ApiController] // API Controller marker
    [Authorize]
    
    public class StockController : ControllerBase
    {

        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

 
        [HttpGet]  // GET request
       
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObject queryObject) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var stockList = await _stockService.GetAllStocksAsync(queryObject);

            return Ok(stockList);
        }

        [HttpGet("{id:int}")] // GET request for details
        public async Task<IActionResult> GetById([FromRoute] int id) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var stock = await _stockService.GetStockByIdAsync(id);

            // Exception handling
            if (stock == null) {
                return NotFound("Stock does not exist");
            } else {
                return Ok(stock); 
            }       
        }

        [HttpPost]  // Create Requests
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO stockDTO) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var create = await _stockService.CreateStockAsync(stockDTO);

            return CreatedAtAction(nameof(GetById), new { id = create.Id }, create);
        }

        [HttpPut] 
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDTO updateDTO) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var stock = await _stockService.UpdateStockAsync(id, updateDTO);

            if (stock == null) {
                return NotFound();
            }

            return Ok(stock);
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> DeleteStock([FromRoute] int id) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON
                        
            await _stockService.DeleteStockAsync(id);

            return NoContent();
        }
    }
}