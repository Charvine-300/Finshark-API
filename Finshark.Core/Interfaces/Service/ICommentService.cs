using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Helpers;

namespace Finshark.Core.Interfaces.Service
{
    public interface ICommentService
    {
        Task<List<CommentDTO>> GetAllCommentsAsync(CommentQueryObject query);  
        
        Task<CommentDTO> GetCommentByIdAsync(int id);

        Task<CommentDTO> CreateCommentForStockAsync(string symbol, CreateCommentDTO commentDTO, string username);

        Task<CommentDTO> UpdateCommentAsync(int id, UpdateCommentDTO commentDTO);

        Task<CommentDTO> DeleteCommentAsync(int id);
    }
}