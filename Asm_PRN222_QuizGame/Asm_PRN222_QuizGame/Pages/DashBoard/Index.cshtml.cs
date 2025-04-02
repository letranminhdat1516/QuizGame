using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.DashBoard
{
    public class IndexModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly IScoreService _scoreService;
        private readonly IGameService _gameService;

        public int QuizCount { get; set; }
        public int QuestionCount { get; set; }
        public int ScoreCount { get; set; }
        public int GameCount { get; set; }
        public Dictionary<string, int> TopTeams { get; set; } = new();

        public IndexModel(
            IQuizService quizService,
            IQuestionService questionService,
            IScoreService scoreService,
            IGameService gameService)
        {
            _quizService = quizService;
            _questionService = questionService;
            _scoreService = scoreService;
            _gameService = gameService;

        }

        public async Task OnGet()
        {
            var quizzes = await _quizService.GetQuizs("", 1, int.MaxValue);
            var questions = await _questionService.GetQuestions("", 1, int.MaxValue);
            var scores = await _scoreService.GetScores("", 1, int.MaxValue);
            var games = await _gameService.GetGames("", 1, int.MaxValue); // Lấy dữ liệu Games

            QuizCount = quizzes.Count();
            QuestionCount = questions.Count();
            GameCount = games.Count();

            // Lấy Top 5 Teams có điểm cao nhất
            TopTeams = scores
       .Where(s => s.TeamId.HasValue) // Lọc bỏ các giá trị null
       .OrderByDescending(s => s.Score.GetValueOrDefault()) // Sắp xếp theo điểm
       .Take(5)
       .ToDictionary(s => s.TeamId.Value.ToString(), s => s.Score.GetValueOrDefault()); // Chuyển kiểu dữ liệu phù hợp

        }
    }
}
