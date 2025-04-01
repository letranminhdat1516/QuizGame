using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IGameLog
    {
        Task<GameLogModel> GetGameLogById(int id);
        Task<IEnumerable<GameLogModel>> GetGameLogs(int pageNumber, int pageSize);
        Task AddGameLog(GameLogModel gameLog);
        Task RemoveGameLog(int id);
        //Task SetActionGameLog(int id, bool status);
        Task<string> CheckAction(int id);
    }
}
