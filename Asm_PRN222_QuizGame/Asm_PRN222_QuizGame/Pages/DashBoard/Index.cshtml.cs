using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.Service;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.DashBoard
{
    public class IndexModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly IScoreService _scoreService;

        public int QuizCount { get; set; }
        public int QuestionCount { get; set; }
        public int ScoreCount { get; set; }

        public IndexModel(IQuizService quizService, IQuestionService questionService, IScoreService scoreService)
        {
            _quizService = quizService;
            _questionService = questionService;
            _scoreService = scoreService;
        }

        public async Task OnGet()
        {
            var quizzes = await _quizService.GetQuizs("", 1, int.MaxValue);
            var questions = await _questionService.GetQuestions("", 1, int.MaxValue);
            var scores = await _scoreService.GetScores("", 1, int.MaxValue);

            QuizCount = quizzes.Count();
            QuestionCount = questions.Count();
            ScoreCount = questions.Count();
        }
    }
}
