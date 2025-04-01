using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface ITeamService
    {
        Task AssignPlayerToTeam(int playerId, int gameId, string teamName);  // Gán người chơi vào đội
    }

}
