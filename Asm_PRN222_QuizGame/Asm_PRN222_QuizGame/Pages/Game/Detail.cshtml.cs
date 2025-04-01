using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class DetailsModel : PageModel
    {
        private readonly IGameService _gameService;

        public DetailsModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public GameModel Game { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Game = await _gameService.GetGameById(id.Value);
                if (Game == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading game details: {ex.Message}";
            }

            return Page();
        }
    }
}
