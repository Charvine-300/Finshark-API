using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Helpers;
using api.Mappers;
using api.Interfaces;
using Finshark.Core.Interfaces.Service;

namespace Finshark.Services.Services
{
    public class StockService: IStockService
    {
        private readonly IStockRepository _stockRepo;
        
        public StockService(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<List<StockDTO>> GetAllStocksAsync(QueryObject query)
        {
            var stocks = await _stockRepo.GetAllAsync(query);
            var stocksList = stocks.Select(x => x.ToStockDTO()).ToList();

            return stocksList;
        }

        public async Task<StockDTO?> GetStockByIdAsync(int id)
        {
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null) {
                return null;
            } else {
                return stock.ToStockDTO();
            }
        }

        public async Task<StockDTO> CreateStockAsync(CreateStockDTO stockDTO)
        {
            var createStock = stockDTO.FromStockDTO();
            await _stockRepo.CreateAsync(createStock);

            return createStock.ToStockDTO();
        }

        public async Task<StockDTO?> UpdateStockAsync(int id, UpdateStockDTO updateDTO)
        {
            var stock = await _stockRepo.UpdateAsync(id, updateDTO);

            if (stock == null) {
                return null;
            } else {
                return stock.ToStockDTO();
            }
        }

        public async Task<StockDTO> DeleteStockAsync(int id)
        {
            var deletedStock = await _stockRepo.DeleteAsync(id); // Assuming this returns a StockDTO
            return deletedStock.ToStockDTO();
        }
    }
}