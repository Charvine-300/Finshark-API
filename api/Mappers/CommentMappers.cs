using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel) {
            return new CommentDTO {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId,
            };
        }

         public static Comment ToCreateCommentDTO(this CreateCommentDTO commentModel, int stockId) {
            return new Comment {
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = stockId,
            };
        }

        public static Comment ToUpdateCommentDTO(this UpdateCommentDTO commentModel) {
            return new Comment {
                Title = commentModel.Title,
                Content = commentModel.Content,
            };
        }
    }
    
}