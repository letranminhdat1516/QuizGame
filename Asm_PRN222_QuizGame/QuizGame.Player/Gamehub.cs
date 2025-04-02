using Microsoft.AspNetCore.SignalR;
using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;
using QuizGame.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizGame.Player
{
    public class GameHub : Hub
    {
        private readonly IPlayerService _playerService;
        private static readonly Dictionary<string, HashSet<string>> _connectedPlayers = new Dictionary<string, HashSet<string>>();
        private static readonly Dictionary<string, Queue<QuestionModel>> _gameQuestions = new Dictionary<string, Queue<QuestionModel>>();

        public GameHub(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task JoinGameGroup(string gamePin)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gamePin);

            // Initialize the player set if it doesn't exist
            if (!_connectedPlayers.ContainsKey(gamePin))
            {
                _connectedPlayers[gamePin] = new HashSet<string>();
            }
        }
        public async Task NotifyPlayerJoined(string pinCode, string playerName)
        {
            await Clients.Group(pinCode).SendAsync("ReceivePlayerJoinedNotification", playerName);
        }
        public async Task PlayerJoined(string gamePin, string playerName)
        {
            if (!_connectedPlayers.ContainsKey(gamePin))
            {
                _connectedPlayers[gamePin] = new HashSet<string>(); ;
            }
            _connectedPlayers[gamePin].Add(playerName);


            await Clients.Group(gamePin).SendAsync("PlayerJoined", playerName);
        }

        public async Task PlayerReady(string gamePin, string playerName)
        {
            await Clients.Group(gamePin).SendAsync("PlayerReady", playerName);
        }

        public async Task CreateTeam(string gamePin, string teamName)
        {
            var team = await _playerService.CreateTeam(gamePin, teamName);

            await Clients.Group(gamePin).SendAsync("TeamCreated", new TeamModel
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                MemberCount = 0
            });
        }

        public async Task JoinTeam(string gamePin, string playerName, int teamId)
        {
            await Clients.Group(gamePin).SendAsync("PlayerJoinedTeam", playerName, teamId);
        }

        public async Task StartGame(string gamePin)
        {
            // Change game status to "Ongoing"
            await _playerService.StartGame(gamePin);

            // Get the game's questions
            var game = await _playerService.GetGameByPinCode(gamePin);
            var questions = await _playerService.GetQuestionsForGame(game.GameId);

            // Store questions for this game
            _gameQuestions[gamePin] = new Queue<QuestionModel>(questions);

            // Notify all clients that the game has started
            await Clients.Group(gamePin).SendAsync("GameStarted");

            // Start the first question after a short delay
            await Task.Delay(3000);
            await SendNextQuestion(gamePin);
        }

        public async Task SendNextQuestion(string gamePin)
        {
            if (_gameQuestions.ContainsKey(gamePin) && _gameQuestions[gamePin].Count > 0)
            {
                var question = _gameQuestions[gamePin].Dequeue();

                // Send the question to all players
                await Clients.Group(gamePin).SendAsync("ReceiveQuestion", question);

                // Schedule ending this question after the time limit
                await Task.Delay(question.TimeLimit * 1000 + 1000); // Add 1 second buffer
                await EndQuestion(gamePin, question.QuestionId);

                // If there are more questions, continue after a delay
                if (_gameQuestions[gamePin].Count > 0)
                {
                    await Task.Delay(5000); // 5 seconds between questions
                    await SendNextQuestion(gamePin);
                }
                else
                {
                    // No more questions, end the game
                    await EndGame(gamePin);
                }
            }
        }

        public async Task SubmitAnswer(string gamePin, string playerName, int questionId, string answer)
        {
            // Record the answer
            var isCorrect = await _playerService.SubmitAnswer(gamePin, playerName, questionId, answer);

            // Acknowledge receipt of the answer
            await Clients.Caller.SendAsync("AnswerReceived");

            // Check if all players have answered
            // This would require tracking active players and their answers
            // For simplicity, we rely on the timer for now
        }

        public async Task EndQuestion(string gamePin, int questionId)
        {
            // Notify all clients that the question has ended
            await Clients.Group(gamePin).SendAsync("QuestionEnded", questionId);

            // Calculate and send scores
            var scores = await CalculateScores(gamePin);
            await Clients.Group(gamePin).SendAsync("UpdateScores", scores);
        }

        public async Task EndGame(string gamePin)
        {
            // Get final scores
            var scores = await CalculateScores(gamePin);

            // Find the winning team
            var winningTeam = scores
                .GroupBy(s => s.TeamId)
                .Select(g => new { TeamId = g.Key, TotalScore = g.Sum(p => p.Score), TeamName = g.First().TeamName })
                .OrderByDescending(t => t.TotalScore)
                .FirstOrDefault();

            string winnerName = winningTeam?.TeamName ?? "No winner";

            // Notify all clients that the game has ended
            await Clients.Group(gamePin).SendAsync("GameEnded", winnerName);

            // Clean up resources
            _connectedPlayers.Remove(gamePin);
            _gameQuestions.Remove(gamePin);
        }

        private async Task<List<PlayerScoreModel>> CalculateScores(string gamePin)
        {
            // This would usually be implemented in the service layer
            // For now, return dummy data
            return new List<PlayerScoreModel>
            {
                new PlayerScoreModel { PlayerName = "Player1", TeamName = "Team A", Score = 100 },
                new PlayerScoreModel { PlayerName = "Player2", TeamName = "Team B", Score = 80 }
            };
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Handle player disconnection
            // For each game the player is in, remove them from the connected players list
            foreach (var game in _connectedPlayers.Keys.ToList())
            {
                if (_connectedPlayers[game].RemoveWhere(name => name == Context.ConnectionId) > 0)
                {
                    await Clients.Group(game).SendAsync("PlayerLeft", Context.ConnectionId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}