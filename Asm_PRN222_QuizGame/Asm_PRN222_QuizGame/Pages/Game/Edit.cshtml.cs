using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class EditModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IQuizService _quizService;

        public EditModel(IGameService gameService, IQuizService quizService)
        {
            _gameService = gameService;
            _quizService = quizService;
        }

        [BindProperty]
        public GameModel Game { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        // Property to hold the list of quizzes
        public List<SelectListItem> Quizzes { get; set; }

        // Method to handle GET request and fetch game details
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

                // Load the list of quizzes for the dropdown
                await LoadQuizzes();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading game: {ex.Message}";
            }

            return Page();
        }

        // Method to handle POST request and save the updated game details
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadQuizzes(); // Reload quizzes if model state is invalid
                return Page();
            }

            // Check if the game name already exists (excluding the current game)
            var existingGame = await _gameService.GetGameByName(Game.GameName);
            if (existingGame != null && existingGame.GameId != Game.GameId)
            {
                ModelState.AddModelError("Game.GameName", "A game with this name already exists.");
                await LoadQuizzes();
                return Page();
            }

            try
            {
                // Call the EditGame method in the service layer to update the game
                await _gameService.EditGame(Game);

                SuccessMessage = "Game updated successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Unexpected error: {ex.Message}";
                await LoadQuizzes();
                return Page();
            }
        }

        private async Task LoadQuizzes()
        {
            // Fetch all quizzes and populate the dropdown
            var quizList = await _quizService.GetQuizs("", 1, int.MaxValue);
            Quizzes = quizList.Select(q => new SelectListItem
            {
                Value = q.QuizId.ToString(),
                Text = q.QuizName
            }).ToList();
        }
    }
}
