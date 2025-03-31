using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IPlayerAnswerService
    {
        Task SubmitAnswer(int playerId, int questionInGameId, string answer);  // Người chơi trả lời câu hỏi và lưu câu trả lời
    }
}
