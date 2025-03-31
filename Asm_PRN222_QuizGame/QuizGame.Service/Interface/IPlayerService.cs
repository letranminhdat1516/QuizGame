using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IPlayerService
    {
        Task<Game> GetGameByPinCode(string pinCode);  // Lấy game từ mã PIN
        Task<QuestionModel> GetNextQuestionForGame(int gameId);  // Lấy câu hỏi tiếp theo cho game
        Task JoinGame(string pinCode, string playerName);
        Task StartGame(int gameId);
    }

}

