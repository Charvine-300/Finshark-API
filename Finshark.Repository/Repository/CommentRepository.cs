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
    public class CommentRepository: ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context; // Dependency injection
        }

        public async Task<List<Comment>> GetCommentsAsync(CommentQueryObject queryObject) {
            var comments = _context.Comment.Include(c => c.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol)) {
                comments = comments.Where(s => s.Stock.Symbol.ToLower() == queryObject.Symbol.ToLower());
            }

            if (queryObject.IsDescending) {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }

            return await comments.ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id) {
            var comment = await _context.Comment.Include(c => c.AppUserId).FirstOrDefaultAsync(x => x.Id == id);

if (comment == null) {
    return null;
}

return comment;
        }
        public async Task<Comment> CreateCommentAsync(Comment commentModel) {
            await _context.Comment.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment comment) {
            var existingComment = await _context.Comment.FindAsync(id);

            if (existingComment == null) {
                return null;
            } 

            existingComment.Title = comment.Title;
            existingComment.Content = comment.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        } 

        public async Task<Comment> DeleteCommentAsync(int id) {
             var existingComment = await _context.Comment.FindAsync(id);

            if (existingComment == null) {
                return null;
            } 

            _context.Comment.Remove(existingComment);
            await _context.SaveChangesAsync();

            return existingComment;
        }
    }
}