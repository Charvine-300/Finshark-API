using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/comments")] // Base URL
    [ApiController] // API Controller marker
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Authorize]
        
        public async Task<IActionResult> GetAllAsync() {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var comments = await _commentRepo.GetCommentsAsync();
            var commentsList = comments.Select(x => x.ToCommentDTO());
            return Ok(commentsList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON
            var comment = await _commentRepo.GetCommentByIdAsync(id);

            if (comment == null) {
                return NotFound();
            } else {
                return Ok(comment.ToCommentDTO());
            }
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateCommentForStock([FromRoute] int stockId, CreateCommentDTO commentDTO) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            if (!await _stockRepo.StockExists(stockId)) {
                return BadRequest("Stock does not exist");
            } else {
                var commentModel = commentDTO.ToCreateCommentDTO(stockId);

                await _commentRepo.CreateCommentAsync(commentModel);

               return Ok(commentModel.ToCommentDTO());
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDTO commentDTO) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var comment = await _commentRepo.UpdateCommentAsync(id, commentDTO.ToUpdateCommentDTO());

            if (comment == null) {
                return NotFound();
            } 

            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> DeleteComment([FromRoute] int id) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON
                        
            var commentModel = await _commentRepo.DeleteCommentAsync(id);

            if (commentModel == null) {
                return NotFound("Comment does not exist");
            } 

            return NoContent();
        }
    }
}