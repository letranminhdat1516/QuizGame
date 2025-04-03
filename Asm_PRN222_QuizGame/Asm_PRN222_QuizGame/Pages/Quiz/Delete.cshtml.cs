using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.Quiz
{
    public class DeleteModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService; // Thêm service để lấy câu hỏi

        public DeleteModel(IQuizService quizService, IQuestionService questionService)
        {
            _quizService = quizService;
            _questionService = questionService;
        }

        [BindProperty]
        public QuizModel Quiz { get; set; }

        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Quiz = await _quizService.GetQuizById(id.Value);
                if (Quiz == null)
                {
                    return NotFound();
                }

                // Lấy danh sách câu hỏi liên quan từ `IQuestionService`
                Questions = (await _questionService.GetQuestions("", 1, int.MaxValue))
                    .Where(q => q.QuizId == id.Value)
                    .ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading quiz: {ex.Message}";
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Quiz = await _quizService.GetQuizById(id.Value);
                if (Quiz == null)
                {
                    return NotFound();
                }

                // Kiểm tra nếu có câu hỏi thì không cho phép xóa
                if (Quiz.Questions.Any())
                {
                    ErrorMessage = "Cannot delete this quiz because it has associated questions.";
                    return Page();
                }

                await _quizService.RemoveQuiz(id.Value);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting quiz: {ex.Message}";
                return Page();
            }
        }
    }
}
