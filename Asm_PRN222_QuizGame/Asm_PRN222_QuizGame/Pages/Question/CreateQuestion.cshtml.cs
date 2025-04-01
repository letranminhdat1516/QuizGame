using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Question
{
    public class CreateQuestionModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public CreateQuestionModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [BindProperty]
        public QuestionModel Question { get; set; } = new QuestionModel();

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Không cần gán QuestionId, để cơ sở dữ liệu tự động sinh
                await _questionService.AddQuestion(Question);
                return RedirectToPage("./QuestionIndex");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating question: {ex.Message}";
                return Page();
            }
        }
    }
}