using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Helpers;

namespace Finshark.Core.Interfaces.Service
{
    public interface IStockService
    {
        
        Task<List<StockDTO>> GetAllStocksAsync(QueryObject query);

        Task<StockDTO?> GetStockByIdAsync(int id);

        Task<StockDTO> CreateStockAsync(CreateStockDTO stockDTO);

        Task<StockDTO?> UpdateStockAsync(int id, UpdateStockDTO stockDTO);

        Task<StockDTO> DeleteStockAsync(int id);      
    }
}