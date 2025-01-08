using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository: IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context; // Dependency injection
        }
        public async Task<List<Stock>> GetAllAsync(QueryObject query) {
            var stocks = _context.Stock.Include(x => x.Comments).ThenInclude(c => c.AppUser).AsQueryable();

            // Checking for query parameters
            if (!string.IsNullOrWhiteSpace(query.CompanyName)) {
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol)) {
                stocks  = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }

            if (query.MinPurchase > 0) {
                stocks = stocks.Where(x => x.Purchase >= query.MinPurchase);
            }

            if (query.MaxPurchase > 0) {
                stocks = stocks.Where(x => x.Purchase <= query.MaxPurchase);
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy)) {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)) {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol); // Sorting data
                }
            }

            var skipNo = (query.PageNumber - 1) * query.PageSize; // Subtracting one from page number to avoid off-by-one error

            return await stocks.Skip(skipNo).Take(query.PageSize).ToListAsync();
        }
        public Task<bool> StockExists(int id) {
            return _context.Stock.AnyAsync(x => x.Id == id);
        }
        public async Task<Stock> GetByIdAsync(int id) {
            var model = await _context.Stock.Include(x => x.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(x => x.Id == id);

            if (model == null) {
                return null;
            } else {
                return model;
            }
        }

        public async Task<Stock> GetBySymbolAsync(string symbol) {
            var model = await _context.Stock.FirstOrDefaultAsync(x => x.Symbol == symbol);

            if (model == null) {
                return null;
            } else {
                return model;
            }
        }
        public async Task<Stock> CreateAsync(Stock stock) {
            await  _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            return stock;
        }
        public async Task<Stock> DeleteAsync(int id) {
            var model = await  _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            var comments = await _context.Comment.Where(c => c.StockId == id).ToListAsync(); // Comments related to the stock

             if (model == null) {
                return null;
            }

            _context.Comment.RemoveRange(comments); // Deleting comments to avoid them being orphaned
            _context.Stock.Remove(model);

            await _context.SaveChangesAsync();

            return model;
        }
        public async Task<Stock> UpdateAsync(int id, UpdateStockDTO stockDTO) {
            var model = await  _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

             if (model == null) {
                return null;
            }

             // Updating record
            model.Symbol = stockDTO.Symbol;
            model.CompanyName = stockDTO.CompanyName;
            model.Industry = stockDTO.Industry;
            model.LastDiv = stockDTO.LastDiv;
            model.Purchase = stockDTO.Purchase;
            model.MarketCap = stockDTO.MarketCap;

            await _context.SaveChangesAsync();

            return model;
        } 
    }
}