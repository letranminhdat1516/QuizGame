using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IPlayerService
    {
        Task<Game> GetGameByPinCode(string pinCode);
        Task<string> JoinGame(string pinCode, string playerName);
        Task<List<TeamModel>> GetTeamsForGame(string pinCode);
        Task<List<PlayerModel>> GetPlayerInTearm(int teamId);
        Task JoinTeam(string pinCode, string playerName, int teamId);
        Task<Team> CreateTeam(string pinCode, string teamName);
        Task<List<QuestionModel>> GetQuestionsForGame(int gameId);
        Task StartGame(string pinCode);
        Task<bool> SubmitAnswer(string pinCode, string playerName, int questionId, string answer);
    }

}

