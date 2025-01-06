using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
                private readonly IFMPService _fmpService;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo,  IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
            _fmpService = fmpService;
        }

        [HttpGet]

        public async  Task<IActionResult> GetUserPortfolios() {
            var username = User.GetUsername();
            var appuser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);

            return Ok(userPortfolio);
        }

        [HttpPost]

        public async Task<IActionResult> CreatePortfolio(string symbol) {
            var username = User.GetUsername();
            var appuser = await _userManager.FindByNameAsync(username);

            // Check for stock in DB
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            // Seed data from external API service and populate the DB with the data
            if (stock == null) {
                stock = await _fmpService.FindStockBySymbolAsnyc(symbol);
                if (stock == null) return BadRequest("Stock does not exist");

                await _stockRepo.CreateAsync(stock);
            }


            // Check if Stock is already in portfolio
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to portfolio");

            // Create portfolio object
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appuser.Id
            };

            await _portfolioRepo.CreateAsync(portfolioModel);

            if (portfolioModel == null) {
                return StatusCode(500, "Could not create portfolio");
            } else {
                return Created();
            }
        }

        [HttpDelete]

        public async Task<IActionResult> DeletePortfolio(string symbol) {
            var username = User.GetUsername();
            var appuser = await _userManager.FindByNameAsync(username);

            // Fetching desired stock
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());

            if (filteredStock.Count() == 1) {
                await _portfolioRepo.DeletePortfolio(appuser, symbol);
            } else return BadRequest("Stock is not in your portfolio");

            return Ok();
        }
    }
}