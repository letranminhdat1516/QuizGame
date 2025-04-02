using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class TeamScoreModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int TotalScore { get; set; }
        public int Rank { get; set; }
        public int MemberCount { get; set; }
        public double AverageScore => MemberCount > 0 ? Math.Round((double)TotalScore / MemberCount, 1) : 0;
        public List<PlayerScoreModel> PlayerScores { get; set; } = new List<PlayerScoreModel>();
    }
}
