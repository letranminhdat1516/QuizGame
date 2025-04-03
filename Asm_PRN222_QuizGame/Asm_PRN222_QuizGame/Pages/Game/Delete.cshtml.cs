using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizGame.Service.Service;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class DeleteModel : PageModel
    {
        private readonly IGameService _gameService;


        public DeleteModel(IGameService gameService, IQuizService quizService)
        {
            _gameService = gameService;
       
        }

        [BindProperty]
        public GameModel Game { get; set; }

        public List<SelectListItem> Quizzes { get; set; }

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
                ErrorMessage = $"Error loading question: {ex.Message}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _gameService.RemoveGame(Game.GameId);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting game: {ex.Message}";
                return Page();
            }
        }
    }
}