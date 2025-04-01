using Microsoft.EntityFrameworkCore;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using QuizGame.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Service
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _uow;
        private int _currentQuestionIndex = 0;
        private readonly Dictionary<int, List<int>> _questionOrder = new();

        public GameService(IUnitOfWork uow, int currentQuestionIndex, Dictionary<int, List<int>> questionOrder)
        {
            _uow = uow;
            _currentQuestionIndex = currentQuestionIndex;
            _questionOrder = questionOrder;
        }

        public async Task<object?> GetNextQuestionAsync(int gameId)
        {
            var game = await _uow.GetRepository<Game>().AsQueryable()
                .Include(g => g.Quiz)
                    .ThenInclude(q => q.Questions)
                .FirstOrDefaultAsync(g => g.GameId == gameId);
            if (game == null) return null;

            if (!_questionOrder.ContainsKey(gameId))
            {
                _questionOrder[gameId] = game.Quiz.Questions.OrderBy(q => Guid.NewGuid()).Select(q => q.QuestionId).ToList();
            }

            if (_currentQuestionIndex >= _questionOrder[gameId].Count)
                return null;

            var questionId = _questionOrder[gameId][_currentQuestionIndex++];
            var question = await _uow.GetRepository<Question>().GetByIdAsync(questionId);

            return new
            {
                questionId = question.QuestionId,
                questionText = question.QuestionText,
                options = new Dictionary<string, string>
                {
                    {"A", "Option A"},
                    {"B", "Option B"},
                    {"C", "Option C"},
                    {"D", "Option D"}
                }
            };
        }

        public async Task<bool> SaveAnswerAsync(string playerName, int gameId, int questionId, string answer, int timeTaken)
        {
            // Đảm bảo Player có namespace và class đúng
               var player = await _uow.GetRepository<QuizGame.Repository.Models.Player>().AsQueryable()
                .Where(p => p.PlayerName == playerName && p.GameId == gameId)  
                .FirstOrDefaultAsync();
            if (player == null) return false;

            var question = await _uow.GetRepository<Question>().GetByIdAsync(questionId);
            bool isCorrect = question.CorrectAnswer.Equals(answer);

            await _uow.GetRepository<PlayerAnswer>().AddAsync(new Repository.Models.PlayerAnswer
            {
                PlayerId = player.PlayerId,  
                QuestionInGameId = questionId, // Giả sử mapping 1:1
                Answer = answer,
                IsCorrect = isCorrect,
                TimeTaken = timeTaken
            });

            await _uow.SaveAsync();
            return isCorrect;
        }

        public async Task<bool> AllPlayersAnsweredAsync(int gameId, int questionId)
        {
            var players = await _uow.GetRepository<QuizGame.Repository.Models.Player>().AsQueryable().Where(p => p.GameId == gameId).ToListAsync();
            var answers = await _uow.GetRepository<PlayerAnswer>().AsQueryable().Where(a => a.QuestionInGameId == questionId).ToListAsync();
            return answers.Select(a => a.PlayerId).Distinct().Count() >= players.Count();
        }

        public async Task<List<object>> CalculateScoresAsync(int gameId, int questionId)
        {
            var answers = await _uow.GetRepository<PlayerAnswer>().AsQueryable().Where(a => a.QuestionInGameId == questionId && a.IsCorrect == true).ToListAsync();
            var result = answers.OrderBy(a => a.TimeTaken).Select((a, i) => new
            {
                PlayerId = a.PlayerId,
                Score = 1000 - (i * 100)  // Ví dụ: người trả lời nhanh nhất được 1000, mỗi người sau giảm 100
            }).ToList<object>();
            return result;
        }

    }
}
