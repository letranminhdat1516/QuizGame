using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Quiz
{
    public class CreateModel : PageModel
    {
        private readonly IQuizService _quizService;

        public CreateModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public QuizModel Quiz { get; set; } = new QuizModel();

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
                await _quizService.AddQuiz(Quiz);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating quiz: {ex.Message}";
                return Page();
            }
        }
    }
}