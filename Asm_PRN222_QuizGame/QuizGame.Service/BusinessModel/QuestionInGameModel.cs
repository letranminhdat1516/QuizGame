using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class QuestionInGameModel
    {
        public int? QuestionId { get; set; }
        public int? GameId { get; set; }
        public int QuestionNumber { get; set; }
    }
}
