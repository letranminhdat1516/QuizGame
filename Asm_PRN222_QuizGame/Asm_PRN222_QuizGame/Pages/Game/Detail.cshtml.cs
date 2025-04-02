using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class DetailsModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IQuizService _quizService; // Inject Quiz Service

        public DetailsModel(IGameService gameService, IQuizService quizService)
        {
            _gameService = gameService;
            _quizService = quizService; // Initialize the quiz service
        }

        public GameModel Game { get; set; }
        public string ErrorMessage { get; set; }
        public List<SelectListItem> Quizzes { get; set; } = new List<SelectListItem>(); // Store Quiz data

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

                // Fetch quizzes to display the correct Quiz Name
                await LoadQuizzes();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading game details: {ex.Message}";
            }

            return Page();
        }

        private async Task LoadQuizzes()
        {
            // Fetch all quizzes and populate the dropdown list
            var quizList = await _quizService.GetQuizs("", 1, int.MaxValue);
            Quizzes = quizList.Select(q => new SelectListItem
            {
                Value = q.QuizId.ToString(),
                Text = q.QuizName
            }).ToList();
        }
    }
}
