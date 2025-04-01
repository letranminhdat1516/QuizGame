using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Quiz
{
    public class EditModel : PageModel
    {
        private readonly IQuizService _quizService;

        public EditModel(IQuizService quizService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _quizService.UpdateQuiz(Quiz);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating quiz: {ex.Message}";
                return Page();
            }
        }
    }
}