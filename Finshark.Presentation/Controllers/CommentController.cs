using api.DTOs.Stock;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/comments")] // Base URL
    [ApiController] // API Controller marker
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        private readonly IFMPService _fmpService;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAllAsync([FromQuery] CommentQueryObject queryObject) {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var comments = await _commentRepo.GetCommentsAsync(queryObject);
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

        [HttpPost]
        [Route("{symbol:alpha}")]
        public async Task<IActionResult> CreateCommentForStock([FromRoute] string symbol, CreateCommentDTO commentDTO) {
                        if(!ModelState.IsValid) return BadRequest(ModelState); // Data validation via JSON

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            // Seed data from external API service and populate the DB with the data
            if (stock == null) {
                stock = await _fmpService.FindStockBySymbolAsnyc(symbol);
                if (stock == null) return BadRequest("Stock does not exist");

                await _stockRepo.CreateAsync(stock);
            }

            // Get user to attach to the comment created
            var username = User.GetUsername();
            var appuser = await _userManager.FindByNameAsync(username); // get user from DB

                var commentModel = commentDTO.ToCreateCommentDTO(stock.Id);

                commentModel.AppUserId = appuser.Id; // Add user ID
        
                await _commentRepo.CreateCommentAsync(commentModel);

               return Ok(commentModel.ToCommentDTO());
            
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