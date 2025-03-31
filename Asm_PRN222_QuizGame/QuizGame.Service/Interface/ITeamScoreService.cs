using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface ITeamScoreService
    {
        Task UpdateTeamScore(int gameId, int questionInGameId, int playerId, bool isCorrect);  // Cập nhật điểm số cho đội
        Task RankTeams(int gameId);  // Xếp hạng các đội sau khi game kết thúc
    }

}
