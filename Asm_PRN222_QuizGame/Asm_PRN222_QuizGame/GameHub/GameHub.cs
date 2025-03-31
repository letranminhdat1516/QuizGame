using Microsoft.AspNetCore.SignalR;
using QuizGame.Service.Interface;
using System.Text.RegularExpressions;

namespace Asm_PRN222_QuizGame.Admin.GameHub
{
    public class GameHub : Hub
    {
        private readonly IGameService _gameService;

        public GameHub(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task JoinGame(string playerName, int gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{gameId}");
            await Clients.Group($"game-{gameId}").SendAsync("PlayerJoined", playerName);
        }

        public async Task StartGame(int gameId)
        {
            var nextQuestion = _gameService.GetNextQuestionAsync(gameId);
            await Clients.Group($"game-{gameId}").SendAsync("NextQuestion", nextQuestion);
        }

        public async Task SubmitAnswer(string playerName, int gameId, int questionId, string answer, int timeTaken)
        {
            var isCorrect = _gameService.SaveAnswerAsync(playerName, gameId, questionId, answer, timeTaken);
            var result = await _gameService.AllPlayersAnsweredAsync(gameId, questionId);
            if (result)
            {
                var scores = _gameService.CalculateScoresAsync(gameId, questionId);
                await Clients.Group($"game-{gameId}").SendAsync("UpdateScoreboard", scores);

                var next = _gameService.GetNextQuestionAsync(gameId);
                if (next != null)
                    await Clients.Group($"game-{gameId}").SendAsync("NextQuestion", next);
                else
                    await Clients.Group($"game-{gameId}").SendAsync("GameEnded");
            }
        }
    }
}
