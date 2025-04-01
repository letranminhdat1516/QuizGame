using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class EditModel : PageModel
    {
        private readonly IGameService _gameService;

        public EditModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
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
                ErrorMessage = $"Error loading game: {ex.Message}";
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
                await _gameService.EditGame(Game);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating game: {ex.Message}";
                return Page();
            }
        }
    }
}
