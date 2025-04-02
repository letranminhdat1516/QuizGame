using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Repository.Models
{
    public class TeamModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int MemberCount { get; set; }
    }
}
