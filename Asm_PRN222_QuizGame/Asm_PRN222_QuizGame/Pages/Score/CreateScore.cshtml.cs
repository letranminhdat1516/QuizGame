using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Score
{
    public class CreateScoreModel : PageModel
    {
        private readonly IScoreService _scoreService;

        public CreateScoreModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [BindProperty]
        public ScoreModel Score { get; set; } = new ScoreModel();

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
                await _scoreService.AddScore(Score);
                return RedirectToPage("./ScoreIndex");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating score: {ex.Message}";
                return Page();
            }
        }
    }
}