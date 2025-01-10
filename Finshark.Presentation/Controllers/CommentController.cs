using api.DTOs.Stock;
using api.Extensions;
using api.Helpers;
using Finshark.Core.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/comments")] // Base URL
    [ApiController] // API Controller marker
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAllAsync([FromQuery] CommentQueryObject queryObject) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var comments = await _commentService.GetAllCommentsAsync(queryObject);
            return Ok(comments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var comment = await _commentService.GetCommentByIdAsync(id);

            if (comment == null) {
                return NotFound();
            } else {
                return Ok(comment);
            }
        }

        [HttpPost]
        [Route("{symbol:alpha}")]
        public async Task<IActionResult> CreateCommentForStock([FromRoute] string symbol, CreateCommentDTO commentDTO) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON
            
            // Get user to attach to the comment created
            var username = User.GetUsername();

            var commentModel = await _commentService.CreateCommentForStockAsync(symbol, commentDTO, username);

            if (commentModel == null) return BadRequest("Stock does not exist");

            return Ok(commentModel);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDTO commentDTO) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var comment = await _commentService.UpdateCommentAsync(id, commentDTO);

            if (comment == null) {
                return NotFound();
            } 

            return Ok(comment);
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> DeleteComment([FromRoute] int id) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON
                        
            var commentModel = await _commentService.DeleteCommentAsync(id);

            if (commentModel == null) {
                return NotFound("Comment does not exist");
            } 

            return NoContent();
        }
    }
}