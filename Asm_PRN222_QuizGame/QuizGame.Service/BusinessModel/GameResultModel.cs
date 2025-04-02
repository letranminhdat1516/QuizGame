using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class GameResultModel
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GamePin { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalQuestions { get; set; }
        public string WinningTeam { get; set; }
        public int WinningTeamId { get; set; }
        public int WinningScore { get; set; }
        public List<TeamScoreModel> TeamScores { get; set; } = new List<TeamScoreModel>();
        public List<PlayerScoreModel> PlayerScores { get; set; } = new List<PlayerScoreModel>();
    }
}
