using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Score
{
    public class DetailsScoreModel : PageModel
    {
        private readonly IScoreService _scoreService;

        public DetailsScoreModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

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
            }

            return Page();
        }
    }
}