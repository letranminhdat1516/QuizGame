using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizGame.Repository;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;
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
        private readonly IMapper _mapper;
        private readonly Dictionary<int, int> _currentQuestionIndex = new(); // Lưu chỉ số câu hỏi hiện tại cho mỗi game
        private readonly Dictionary<int, List<int>> _questionOrder = new(); // Lưu thứ tự câu hỏi cho mỗi game

        public GameService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
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

            if (!_currentQuestionIndex.ContainsKey(gameId))
            {
                _currentQuestionIndex[gameId] = 0;
            }

            if (_currentQuestionIndex[gameId] >= _questionOrder[gameId].Count)
            {
                return null;
            }


            var questionId = _questionOrder[gameId][_currentQuestionIndex[gameId]];
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
        public async Task AddGame(GameModel game)
        {
            try
            {
                // Kiểm tra xem GamePin có được cung cấp không
                if (string.IsNullOrEmpty(game.GamePin))
                {
                    throw new Exception("Game PIN is required.");
                }

                // Kiểm tra xem GamePin đã tồn tại chưa
                var existingGame = await _uow.GetRepository<Game>().FindAsync(g => g.GamePin == game.GamePin);
                if (existingGame.Any())
                {
                    throw new Exception($"Game PIN {game.GamePin} already exists. Please choose a different PIN.");
                }

                var gameEntity = _mapper.Map<Game>(game);
                var gameRepository = _uow.GetRepository<Game>();
                await gameRepository.AddAsync(gameEntity);
                await _uow.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding game: {ex.Message}", ex);
            }
        }


        public async Task<GameModel> GetGameByPin(string gamePin)
        {
            try
            {
                if (string.IsNullOrEmpty(gamePin))
                {
                    throw new Exception("Game PIN cannot be empty.");
                }

                var gameRepository = _uow.GetRepository<Game>();
                var game = await gameRepository.FindAsync(g => g.GamePin == gamePin);
                var result = game.FirstOrDefault();
                if (result == null)
                {
                    throw new Exception($"Game with PIN {gamePin} not found.");
                }
                return _mapper.Map<GameModel>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving game by PIN: {ex.Message}", ex);
            }
        }

        public async Task<QuizModel> GetQuizByGamePin(string gamePin)
        {
            try
            {
                if (string.IsNullOrEmpty(gamePin))
                {
                    throw new Exception("Game PIN cannot be empty.");
                }

                var gameRepository = _uow.GetRepository<Game>();
                var quizRepository = _uow.GetRepository<Quiz>();

                // Lấy Game theo GamePin và bao gồm Quiz
                var gameQuery = gameRepository.AsQueryable()
                    .Include(g => g.Quiz)
                    .Where(g => g.GamePin == gamePin);

                var game = await gameQuery.FirstOrDefaultAsync();

                if (game == null)
                {
                    throw new Exception($"Game with PIN {gamePin} not found.");
                }

                if (game.QuizId == null || game.Quiz == null)
                {
                    throw new Exception($"Quiz not found for Game with PIN {gamePin}.");
                }

                return _mapper.Map<QuizModel>(game.Quiz);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving quiz by game PIN: {ex.Message}", ex);
            }
        }

        public async Task<GameModel> GetGameById(int gameId)
        {
            try
            {
                var gameRepository = _uow.GetRepository<Game>();
                var game_Temp = await gameRepository.GetByIdAsync(gameId);
                return _mapper.Map<GameModel>(game_Temp);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving question: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<GameModel>> GetGames(string search, int pageNumber, int pageSize)
        {
            try
            {
                var gameRepository = _uow.GetRepository<Game>();

                var query = gameRepository.AsQueryable();


                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(q => q.GameName.Contains(search) || q.GamePin.Contains(search));
                }

                // Sắp xếp theo ID để đảm bảo kết quả đúng thứ tự
                query = query.OrderBy(q => q.GameId);

                // Áp dụng phân trang
                int skip = (pageNumber - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);

                var games = await query.ToListAsync();
                return _mapper.Map<IEnumerable<GameModel>>(games);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving questions: {ex.Message}", ex);
            }
        }

        public async Task EditGame(GameModel game)
        {
            try
            {
                var gameRepository = _uow.GetRepository<Game>();
                var game_Temp = _mapper.Map<Game>(game);
                await gameRepository.UpdateAsync(game_Temp);
                await _uow.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating question: {ex.Message}", ex);
            }
        }

        public async Task RemoveGame(int id)
        {
            try
            {
                var gameRepository = _uow.GetRepository<Game>();
                await gameRepository.DeleteAsync(id);
                await _uow.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing question: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalGamesCount(string search)
        {
            var gameRepository = _uow.GetRepository<Game>();
            var query = gameRepository.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.GameName.Contains(search) || q.GamePin.Contains(search));
            }

            return await query.CountAsync();
        }

    }

}
