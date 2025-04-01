using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class GameModel
    {
        public int GameId { get; set; }

        public string GameName { get; set; }

        public int? HostId { get; set; }

        [Required(ErrorMessage = "Game PIN is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Game PIN must be a 6-digit number.")]
        public string GamePin { get; set; }

        public string Status { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? QuizId { get; set; }
    }
}
