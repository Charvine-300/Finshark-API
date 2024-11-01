using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stockModel) {
            return new StockDTO {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                Purchase = stockModel.Purchase,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(x => x.ToCommentDTO()).ToList(),
            };
        }

        public static Stock FromStockDTO(this CreateStockDTO stockDTO) {
            return new Stock {
                Symbol = stockDTO.Symbol,
                CompanyName = stockDTO.CompanyName,
                Industry = stockDTO.Industry,
                LastDiv = stockDTO.LastDiv,
                Purchase = stockDTO.Purchase,
                MarketCap = stockDTO.MarketCap
            };
        }
    }
}