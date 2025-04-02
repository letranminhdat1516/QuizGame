using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuizGame.Player;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;
using QuizGame.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;
public class PlayerService : IPlayerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<GameHub> _hubContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PlayerService(IUnitOfWork unitOfWork, IHubContext<GameHub> hubContext, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _httpContextAccessor = httpContextAccessor;
    }

    // Lấy game theo mã PIN
    public async Task<Game> GetGameByPinCode(string pinCode)
    {
        var gameRepository = _unitOfWork.GetRepository<Game>();
        var game = await gameRepository.AsQueryable()
            .FirstOrDefaultAsync(g => g.GamePin == pinCode && g.Status == "Waiting");

        if (game == null)
        {
            throw new ArgumentException("Invalid PIN or game already started.");
        }

        return game;
    }

    // Get all teams for a specific game
    public async Task<List<TeamModel>> GetTeamsForGame(string pinCode)
    {
        var game = await GetGameByPinCode(pinCode);

        var teamRepository = _unitOfWork.GetRepository<Team>();
        var teams = await teamRepository.AsQueryable()
            .Where(t => t.GameId == game.GameId)
            .Select(t => new TeamModel
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName,
                MemberCount = t.Players.Count
            })
            .ToListAsync();

        return teams;
    }

    // Join game
    public async Task<QuizGame.Repository.Models.Player> JoinGame(string pinCode, string playerName)
    {
        var game = await GetGameByPinCode(pinCode);
        if (game == null) throw new Exception("Game not found");

        // Check if player name is already taken in this game
        var playerRepository = _unitOfWork.GetRepository<QuizGame.Repository.Models.Player>();
        var existingPlayer = await playerRepository.AsQueryable()
            .FirstOrDefaultAsync(p => p.GameId == game.GameId && p.PlayerName == playerName);

        if (existingPlayer != null)
        {
            throw new ArgumentException("Player name already taken. Please choose another name.");
        }

        // Create player without assigning to a team yet
        var player = new QuizGame.Repository.Models.Player
        {
            PlayerName = playerName,
            GameId = game.GameId,
            TeamId = null, // Will be assigned when player chooses a team
            PinCode = pinCode,
            JoinTime = DateTime.Now
        };

        await playerRepository.AddAsync(player);
        await _unitOfWork.SaveAsync();

        // Send notification via SignalR
        await _hubContext.Clients.Group(pinCode).SendAsync("PlayerJoined", playerName);

        return player;
    }

    // Join a team
    public async Task JoinTeam(string pinCode, string playerName, int teamId)
    {
        var game = await GetGameByPinCode(pinCode);

        // Find the player
        var playerRepository = _unitOfWork.GetRepository<QuizGame.Repository.Models.Player>();
        var player = await playerRepository.AsQueryable()
            .FirstOrDefaultAsync(p => p.GameId == game.GameId && p.PlayerName == playerName);

        if (player == null)
            throw new ArgumentException("Player not found");

        // Verify the team exists and belongs to this game
        var teamRepository = _unitOfWork.GetRepository<Team>();
        var team = await teamRepository.AsQueryable()
            .FirstOrDefaultAsync(t => t.TeamId == teamId && t.GameId == game.GameId);

        if (team == null)
            throw new ArgumentException("Team not found or not part of this game");

        // Update player's team
        player.TeamId = teamId;
        await _unitOfWork.SaveAsync();

        // Notify via SignalR
        await _hubContext.Clients.Group(pinCode).SendAsync("PlayerJoinedTeam", playerName, teamId);
    }

    // Create a new team
    public async Task<Team> CreateTeam(string pinCode, string teamName)
    {
        var game = await GetGameByPinCode(pinCode);

        var teamRepository = _unitOfWork.GetRepository<Team>();
        var existingTeam = await teamRepository.AsQueryable()
            .FirstOrDefaultAsync(t => t.GameId == game.GameId && t.TeamName == teamName);

        if (existingTeam != null)
        {
            throw new ArgumentException("Team name already taken. Please choose another name.");
        }
        var team = new Team
        {
            TeamName = teamName,
            GameId = game.GameId,
            CreatedAt = DateTime.Now
        };

        await teamRepository.AddAsync(team);
        await _unitOfWork.SaveAsync();

        // Notify via SignalR
        await _hubContext.Clients.Group(pinCode).SendAsync("TeamCreated", new TeamModel
        {
            TeamId = team.TeamId,
            TeamName = team.TeamName,
            MemberCount = 0
        });

        return team;
    }

    // Get questions for a game with their timing information
    public async Task<List<QuestionModel>> GetQuestionsForGame(int gameId)
    {
        var questionInGameRepository = _unitOfWork.GetRepository<QuestionInGame>();
        var questionRepository = _unitOfWork.GetRepository<Question>();

        var questionsInGame = await questionInGameRepository.AsQueryable()
            .Where(qig => qig.GameId == gameId)
            .OrderBy(qig => qig.QuestionNumber)
            .ToListAsync();

        var result = new List<QuestionModel>();
        foreach (var qig in questionsInGame)
        {
            var question = await questionRepository.GetByIdAsync(qig.QuestionId ?? 0);
            result.Add(new QuestionModel
            {
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText,
                CorrectAnswer = question.CorrectAnswer,
                TimeLimit = question.TimeLimit ?? 30, // Default 30 seconds if not specified
            });
        }

        return result;
    }

    // Start the game
    public async Task StartGame(string pinCode)
    {
        var game = await GetGameByPinCode(pinCode);

        // Update game status
        var gameRepository = _unitOfWork.GetRepository<Game>();
        game.Status = "Ongoing";
        await _unitOfWork.SaveAsync();

        // Notify via SignalR to start the game
        await _hubContext.Clients.Group(pinCode).SendAsync("GameStarted");
    }

    // Record a player's answer
    public async Task<bool> SubmitAnswer(string pinCode, string playerName, int questionId, string answer)
    {
        var game = await GetGameByPinCode(pinCode);

        // Find the player
        var playerRepository = _unitOfWork.GetRepository<Player>();
        var player = await playerRepository.AsQueryable()
            .FirstOrDefaultAsync(p => p.GameId == game.GameId && p.PlayerName == playerName);

        if (player == null)
            throw new ArgumentException("Player not found");

        // Record the answer
        var answerRepository = _unitOfWork.GetRepository<PlayerAnswer>();
        var playerAnswer = new PlayerAnswer
        {
            PlayerId = player.PlayerId,
            QuestionInGameId = questionId,
            Answer = answer,
            TimeTaken = 10//data temp
        };

        await answerRepository.AddAsync(playerAnswer);
        await _unitOfWork.SaveAsync();

        // Check if answer is correct
        var questionRepository = _unitOfWork.GetRepository<Question>();
        var question = await questionRepository.GetByIdAsync(questionId);

        return answer == question.CorrectAnswer;
    }
}