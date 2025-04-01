using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.Quiz
{
    public class DetailsModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;

        public DetailsModel(IQuizService quizService, IQuestionService questionService)
        {
            _quizService = quizService;
            _questionService = questionService;
        }

        public QuizModel Quiz { get; set; }
        public IList<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
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

                // Lấy danh sách câu hỏi (không phân trang để đơn giản)
                Questions = (await _questionService.GetQuestions("", 1, int.MaxValue))
                    .Where(q => q.QuizId == id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading quiz details: {ex.Message}";
            }

            return Page();
        }
    }
}