using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IScoreService
    {
        Task<ScoreModel> GetScoreById(int id);
        Task<IEnumerable<ScoreModel>> GetScores(string search, int pageNumber, int pageSize);
        Task<int> GetTotalScoresCount(string search);
        Task AddScore(ScoreModel score);
        Task UpdateScore(ScoreModel score);
        Task RemoveScore(int id);

    }
}
