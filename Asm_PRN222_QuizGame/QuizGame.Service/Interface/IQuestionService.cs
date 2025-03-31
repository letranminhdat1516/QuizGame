using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizGame.Service.Interface
{
    public interface IQuestionService
    {
        Task<QuestionModel> GetQuestionById(int id);
        Task<IEnumerable<QuestionModel>> GetQuestions(string search , int pageNumber, int pageSize);
        Task AddQuestion(QuestionModel question);
        Task UpdateQuestion(QuestionModel question);
        Task RemoveQuestion(int id);
        Task SetStatusQuestion(int id, bool status);
        Task<bool> CheckStatus(int id);
<<<<<<< HEAD
        Task<int> GetTotalQuestionsCount(string search);
=======
        Task<Game> GetGameByPinCode(string pinCode);  // Lấy game bằng mã PIN
        Task<QuestionModel> GetNextQuestionForGame(int gameId, int questionNumber);  // Lấy câu hỏi tiếp theo cho game
        Task<IEnumerable<QuestionModel>> GetQuestionsWithQuizId(int? quizId);
        Task AddQuestionInGame(QuestionInGameModel questionInGameModel);
>>>>>>> origin/NguyenHP
    }
}
