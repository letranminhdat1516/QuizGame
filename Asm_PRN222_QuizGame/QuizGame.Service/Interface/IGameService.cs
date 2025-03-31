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
        //Task<bool> AllPlayersAnsweredAsync(int gameId, int questionId);
        Task<List<object>> CalculateScoresAsync(int gameId, int questionId);
    }
}
