using QuizGame.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class ScoreModel
    {

        public int TeamScoreId { get; set; }

        public int? TeamId { get; set; }

        public int? QuestionInGameId { get; set; }

        public int? Score { get; set; }

        public int? Rank { get; set; }
    }
}
