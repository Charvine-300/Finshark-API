

using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task <Comment> CreateCommentAsync(Comment commentModel);
        Task <Comment?> UpdateCommentAsync(int id, Comment commentModel);
        Task<Comment> DeleteCommentAsync(int id);
    }
}