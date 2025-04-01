using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using QuizGame.Service.Interface;

namespace QuizGame.Service.Service
{
    public class PlayerAnswerService : IPlayerAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlayerAnswerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> SubmitAnswer(int playerId, int questionId, string answer, int timeTaken)
        {
            // Đảm bảo Player có namespace và class đúng
            var player = await _unitOfWork.GetRepository<QuizGame.Repository.Models.Player>().AsQueryable()
             .Where(p => p.PlayerId == 1)
             .FirstOrDefaultAsync();
            if (player == null) return false;

            var question = await _unitOfWork.GetRepository<Question>().GetByIdAsync(questionId);
            bool isCorrect = question.CorrectAnswer.Equals(answer);

            await _unitOfWork.GetRepository<PlayerAnswer>().AddAsync(new Repository.Models.PlayerAnswer
            {
                PlayerId = player.PlayerId,
                QuestionInGameId = questionId, // Giả sử mapping 1:1
                Answer = answer,
                IsCorrect = isCorrect,
                TimeTaken = timeTaken
            });

            await _unitOfWork.SaveAsync();
            return isCorrect;
        }

       
    }
}