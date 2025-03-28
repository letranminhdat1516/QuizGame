using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class GameLogModel
    {
        public int LogId { get; set; }

        public int? GameId { get; set; }

        public DateTime? ActionTime { get; set; }
    }
}
