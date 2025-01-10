using api.Mappers;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Extensions;
using Finshark.Core.Interfaces.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.Models;

namespace Finshark.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepo;

        private readonly IStockRepository _stockRepo;

        private readonly IFMPService _fmpService;

        private readonly UserManager<AppUser> _userManager;

        public CommentService(UserManager<AppUser> userManager, IFMPService fmpService, ICommentRepository commentRepo, IStockRepository stockRepo)
        {
             _fmpService = fmpService;
              _userManager = userManager;
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }


        public async Task<List<CommentDTO>> GetAllCommentsAsync(CommentQueryObject query)
        {
            var comments = await _commentRepo.GetCommentsAsync(query);
            var commentsList = comments.Select(x => x.ToCommentDTO());

            return commentsList.ToList();
        }

        public async Task<CommentDTO> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id);
            if (comment == null) {
                return null;
            } else {
                return comment.ToCommentDTO();
            }
        }

        public async Task<CommentDTO> CreateCommentForStockAsync(string symbol, CreateCommentDTO commentDTO, string username)
        {
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            // Seed data from external API service and populate the DB with the data
            if (stock == null) {
                stock = await _fmpService.FindStockBySymbolAsnyc(symbol);
                if (stock == null) return null;

                await _stockRepo.CreateAsync(stock);
            }

            var appuser = await _userManager.FindByNameAsync(username); // get user from DB

            var commentModel = commentDTO.ToCreateCommentDTO(stock.Id);

            commentModel.AppUserId = appuser.Id; // Add user ID
        
            await _commentRepo.CreateCommentAsync(commentModel); 

            return commentModel.ToCommentDTO();
        }


        public async Task<CommentDTO> DeleteCommentAsync(int id)
        {
            var commentModel = await _commentRepo.DeleteCommentAsync(id);

            if (commentModel == null) return null;

            return commentModel.ToCommentDTO();
        }



        public async Task<CommentDTO> UpdateCommentAsync(int id, UpdateCommentDTO commentDTO)
        {
            var comment = await _commentRepo.UpdateCommentAsync(id, commentDTO.ToUpdateCommentDTO());

            if (comment == null) {
                return null;
            } else {
                return comment.ToCommentDTO();
            }
        }
    }
}