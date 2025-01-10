using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace Finshark.Core.Interfaces.Service
{
    public interface IPortfolioService
    {
        Task<List<Stock>> GetAllUserPortfoliosAsync(string username);

        Task<object> CreatePortfolioAsync(string symbol, string username);

        Task<bool> DeletePortfolio(string symbol, string username);
    }
}