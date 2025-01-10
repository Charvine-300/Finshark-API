using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Finshark.Core.Interfaces.Service;
using Microsoft.AspNetCore.Identity;

namespace Finshark.Services.Services
{
    public class PortfolioService: IPortfolioService
    {
         private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;
        
        public PortfolioService(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo,  IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
            _fmpService = fmpService;
        }

        public async Task<List<Stock>> GetAllUserPortfoliosAsync(string username)
        {
            var appuser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);

            if (userPortfolio == null) return null;

            return userPortfolio;
        }

        public async Task<object> CreatePortfolioAsync(string symbol, string username)
        {
            var appuser = await _userManager.FindByNameAsync(username);

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            // Seed data from external API service and populate the DB with the data
            if (stock == null) {
                stock = await _fmpService.FindStockBySymbolAsnyc(symbol);
                if (stock == null) return null;

                await _stockRepo.CreateAsync(stock);
            }

            // Check if Stock is already in portfolio
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return "Cannot add same stock to portfolio";

            // Create portfolio object
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appuser.Id
            };

            await _portfolioRepo.CreateAsync(portfolioModel);

            if (portfolioModel == null) {
                return "Could not create portfolio";
            } else {
                return portfolioModel;
            }
        }

        public async Task<bool> DeletePortfolio(string symbol, string username)
        {
            var appuser = await _userManager.FindByNameAsync(username);

            // Fetching desired stock
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appuser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());

              if (filteredStock.Count() == 1)
    {
        await _portfolioRepo.DeletePortfolio(appuser, symbol);
        return true;  // Return true if the deletion was successful
    }
    else
    {
        return false;  // Return false if the stock was not found
    }
        }
    }
}