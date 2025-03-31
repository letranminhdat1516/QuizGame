using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizGame.Service.Interface;

namespace QuizGame.Service.Service
{
    public class PlayerAnswerService : IPlayerAnswerService
    {
        public Task SubmitAnswer(int playerId, int questionInGameId, string answer)
        {
            throw new NotImplementedException();
        }
    }
}
