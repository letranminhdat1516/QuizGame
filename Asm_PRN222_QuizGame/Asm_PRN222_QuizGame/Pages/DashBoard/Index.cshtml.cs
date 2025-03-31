using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.DashBoard
{
    public class IndexModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;

        public int QuizCount { get; set; }
        public int QuestionCount { get; set; }

        public IndexModel(IQuizService quizService, IQuestionService questionService)
        {
            _quizService = quizService;
            _questionService = questionService;
        }

        public async Task OnGet()
        {
            var quizzes = await _quizService.GetQuizs("", 1, int.MaxValue);
            var questions = await _questionService.GetQuestions("", 1, int.MaxValue);

            QuizCount = quizzes.Count();
            QuestionCount = questions.Count();
        }
    }
}
