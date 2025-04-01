using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Quiz
{
    public class DeleteModel : PageModel
    {
        private readonly IQuizService _quizService;

        public DeleteModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public QuizModel Quiz { get; set; }

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