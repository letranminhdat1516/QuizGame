using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IGameService
    {
        Task<object?> GetNextQuestionAsync(int gameId);
        Task<bool> SaveAnswerAsync(string playerName, int gameId, int questionId, string answer, int timeTaken);
        Task<bool> AllPlayersAnsweredAsync(int gameId, int questionId);
        Task AddGame(GameModel game);
        Task<List<object>> CalculateScoresAsync(int gameId, int questionId);
        Task<GameModel> GetGameByPin(string gamePin); // Lấy Game theo GamePin (dành cho người dùng)
        Task<QuizModel> GetQuizByGamePin(string gamePin); // Lấy Quiz theo GamePin (dành cho người dùng)
        Task<GameModel> GetGameById(int gameId);
        Task<IEnumerable<GameModel>> GetGames(string search, int pageNumber, int pageSize);
        Task EditGame(GameModel game);
        Task RemoveGame(int gameId);
        Task<int> GetTotalGamesCount(string search);
        Task<GameModel> GetGameByName(string gameName);
    }
}

