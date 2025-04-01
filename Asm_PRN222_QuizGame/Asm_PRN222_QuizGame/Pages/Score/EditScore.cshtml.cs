using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Score
{
    public class EditScoreModel : PageModel
    {
        private readonly IScoreService _scoreService;

        public EditScoreModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [BindProperty]
        public ScoreModel Score { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Score = await _scoreService.GetScoreById(id.Value);
                if (Score == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading score: {ex.Message}";
                return Page();
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
                await _scoreService.UpdateScore(Score);
                return RedirectToPage("./ScoreIndex");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating score: {ex.Message}";
                return Page();
            }
        }
    }
}