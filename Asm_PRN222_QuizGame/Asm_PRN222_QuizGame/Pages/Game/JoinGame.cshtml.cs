using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Pages
{
    public class JoinGameModel : PageModel
    {
        private readonly IGameService _gameService;

        public JoinGameModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public QuizModel Quiz { get; set; }
        public int GameId { get; set; }
        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string gamePin)
        {
            if (string.IsNullOrEmpty(gamePin))
            {
                ErrorMessage = "Please enter a Game PIN.";
                return Page();
            }

            try
            {
                var game = await _gameService.GetGameByPin(gamePin);
                if (game == null)
                {
                    ErrorMessage = "Invalid Game PIN.";
                    return Page();
                }

                Quiz = await _gameService.GetQuizByGamePin(gamePin);
                if (Quiz == null)
                {
                    ErrorMessage = "Quiz not found for this game.";
                    return Page();
                }

                GameId = game.GameId;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error joining game: {ex.Message}";
                return Page();
            }

            return Page();
        }
    }
}