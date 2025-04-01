using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class CreateGameModel : PageModel
    {
        private readonly IGameService _gameService;

        public CreateGameModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public GameModel Game { get; set; } = new GameModel();

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

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
                await _gameService.AddGame(Game);
                var createdGame = await _gameService.GetGameByPin(Game.GamePin);
                SuccessMessage = $"Game created successfully! Game PIN: {createdGame.GamePin}";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating game: {ex.Message}";
                return Page();
            }
        }
    }
}