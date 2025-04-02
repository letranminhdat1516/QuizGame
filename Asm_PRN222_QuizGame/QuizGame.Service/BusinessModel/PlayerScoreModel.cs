using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class PlayerScoreModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int? TeamId { get; set; }
        public string TeamName { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public double AccuracyPercentage => TotalQuestions > 0 ? Math.Round((double)CorrectAnswers / TotalQuestions * 100, 1) : 0;
    }
}
