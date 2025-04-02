using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class CreateGameModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IQuizService _quizService;

        public CreateGameModel(IGameService gameService, IQuizService quizService)
        {
            _gameService = gameService;
            _quizService = quizService;
        }

        [BindProperty]
        public GameModel Game { get; set; } = new GameModel();

        public List<SelectListItem> Quizzes { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadQuizzes();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadQuizzes(); // Reload quizzes to repopulate dropdown if validation fails
                return Page();
            }

            try
            {
                var existingGame = await _gameService.GetGameByName(Game.GameName);
                if (existingGame != null)
                {
                    ModelState.AddModelError("Game.GameName", "A game with this name already exists.");
                    await LoadQuizzes();
                    return Page();
                }

                await _gameService.AddGame(Game);
                SuccessMessage = "Game created successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unexpected error: {ex.Message}");
                await LoadQuizzes();
                return Page();
            }
        }

        private async Task LoadQuizzes()
        {
            var quizList = await _quizService.GetQuizs("", 1, int.MaxValue); // Fetch all quizzes
            Quizzes = quizList.Select(q => new SelectListItem
            {
                Value = q.QuizId.ToString(),
                Text = q.QuizName
            }).ToList();
        }
    }
}
