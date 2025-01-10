using api.Extensions;
using Finshark.Core.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController: ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]

        public async  Task<IActionResult> GetUserPortfolios() {
            var username = User.GetUsername();

            var userPortfolio = await _portfolioService.GetAllUserPortfoliosAsync(username);

            if (userPortfolio == null) return null;

            return Ok(userPortfolio);
        }

        [HttpPost]

        public async Task<IActionResult> CreatePortfolio(string symbol) {
            var username = User.GetUsername();
            var portfolioModel = _portfolioService.CreatePortfolioAsync(symbol, username);

                if (portfolioModel == null) {
            return BadRequest("Stock does not exist");
        } else if (portfolioModel.ToString() == "Cannot add same stock to portfolio") {
            return BadRequest("Cannot add same stock to portfolio");
        } else if (portfolioModel.ToString() == "Could not create portfolio") {
            return StatusCode(500, "Could not create portfolio");
        } else {
            return Created();
        }
        }

        [HttpDelete]

        public async Task<IActionResult> DeletePortfolio(string symbol) {
            var username = User.GetUsername();
            var result = await _portfolioService.DeletePortfolio(symbol, username);

            if (result)
            {
                return Ok();
            }
            else
            {
            return BadRequest("Stock is not in your portfolio");
            }
        }
    }
}