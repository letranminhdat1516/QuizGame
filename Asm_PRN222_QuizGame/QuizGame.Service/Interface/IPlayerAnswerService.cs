using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IPlayerAnswerService
    {
        Task<bool> SubmitAnswer(int playerId, int questionId, string answer, int timeTaken);  // Người chơi trả lời câu hỏi và lưu câu trả lời
    }
}
