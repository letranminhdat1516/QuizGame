using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IGameLogService
    {
        Task AddGameLog(int gameId, string action);  // Thêm log cho các hành động trong game
    }

}
