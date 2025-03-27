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
        Task<IEnumerable<QuestionModel>> GetQuestions(string search, int pageNumber, int pageSize);
        Task AddQuestion(QuestionModel question);
        Task UpdateQuestion(QuestionModel question);
        Task RemoveQuestion(int id);
        Task SetStatusQuestion(int id, bool status);
        Task<bool> CheckStatus(int id);
    }
}
