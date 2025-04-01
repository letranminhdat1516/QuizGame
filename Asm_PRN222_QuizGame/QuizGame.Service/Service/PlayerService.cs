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

    // Lấy câu hỏi cho game
    public async Task<QuestionModel> GetNextQuestionForGame(int gameId)
    {
        var questionInGameRepository = _unitOfWork.GetRepository<QuestionInGame>();
        var questionInGame = await questionInGameRepository.AsQueryable()
            .FirstOrDefaultAsync(qig => qig.GameId == gameId && qig.QuestionNumber == 1); // Lấy câu hỏi đầu tiên (có thể thay đổi logic lấy câu hỏi khác)

        if (questionInGame == null)
        {
            return null;  // Nếu không còn câu hỏi, trả về null
        }

        var questionRepository = _unitOfWork.GetRepository<Question>();
        var question = await questionRepository.GetByIdAsync(questionInGame.QuestionInGameId);

        return new QuestionModel
        {
            QuestionId = question.QuestionId,
            QuestionText = question.QuestionText,
            CorrectAnswer = question.CorrectAnswer
        };
    }

    // Join game
    public async Task JoinGame(string pinCode, string playerName)
    {
        var game = await GetGameByPinCode(pinCode);
        if (game == null) throw new Exception("Game not found");

        // Thêm player vào game
        var teamRepository = _unitOfWork.GetRepository<Team>();
        var team = await teamRepository.AsQueryable()
            .FirstOrDefaultAsync(t => t.GameId == game.GameId);

        if (team == null)
        {
            team = new Team
            {
                TeamName = $"{playerName}'s Team",
                GameId = game.GameId,
                CreatedAt = DateTime.Now
            };
            await teamRepository.AddAsync(team);
            await _unitOfWork.SaveAsync();
        }

        var playerRepository = _unitOfWork.GetRepository<Player>();
        var player = new Player
        {
            PlayerName = playerName,  // Lưu PlayerName vào bảng Player
            GameId = game.GameId,
            TeamId = team.TeamId,
            PinCode = pinCode,
            JoinTime = DateTime.Now
        };

        await playerRepository.AddAsync(player);
        await _unitOfWork.SaveAsync();

        // Gửi thông báo player đã tham gia
        await _hubContext.Clients.Group(pinCode).SendAsync("PlayerJoined", playerName);
    }

    // Khi game bắt đầu, gửi câu hỏi cho player
    public async Task StartGame(int gameId)
    {
        var gameRepository = _unitOfWork.GetRepository<Game>();
        var game = await gameRepository.GetByIdAsync(gameId);

        // Kiểm tra câu hỏi từ cơ sở dữ liệu
        var questionRepository = _unitOfWork.GetRepository<Question>();
        var questions = await questionRepository.AsQueryable()
            .Where(q => q.QuizId == game.QuizId)
            .ToListAsync();
        if (game == null)
        {
            throw new InvalidOperationException("Game not found.");
        }
        if (!questions.Any())
        {
            throw new InvalidOperationException("No questions available for the game.");
        }

        // Lấy câu hỏi đầu tiên
        var firstQuestion = questions.FirstOrDefault();

        if (firstQuestion != null)
        {
            await _hubContext.Clients.Group(gameId.ToString()).SendAsync("ReceiveQuestion", gameId, firstQuestion.QuestionText);
        }

        // Cập nhật trạng thái game
        game.Status = "Ongoing";
        await _unitOfWork.SaveAsync();
    }
}