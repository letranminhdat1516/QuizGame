using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class JoinGameRequest
    {
        public string PinCode { get; set; }
        public string PlayerName { get; set; }
    }
}
