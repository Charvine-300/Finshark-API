using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?>  GetByIdAsync(int id); // Question mark is there because firstordefualt controller action can be NULL
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockDTO stockDTO);
        Task<Stock> DeleteAsync(int id);

    }
}
