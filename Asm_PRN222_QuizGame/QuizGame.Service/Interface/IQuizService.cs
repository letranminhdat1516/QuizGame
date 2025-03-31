using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IQuizService
    {
        Task<QuizModel> GetQuizById(int id);
        Task<IEnumerable<QuizModel>> GetQuizs(string search, int pageNumber, int pageSize);
        Task AddQuiz(QuizModel quiz);
        Task UpdateQuiz(QuizModel quiz);
        Task RemoveQuiz(int id);
        //Task SetStatusQuiz(int id, bool status);
        //Task<bool> CheckStatus(int id);
        Task<int> GetTotalQuizCount(string search);
    }
}
