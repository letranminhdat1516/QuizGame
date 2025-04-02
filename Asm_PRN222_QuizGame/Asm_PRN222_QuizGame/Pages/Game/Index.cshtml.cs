using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Asm_PRN222_QuizGame.Admin.Pages.Game
{
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;

        public IndexModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IList<GameModel> Games { get; set; } = new List<GameModel>();

        public GameListModel GameList { get; set; } = new GameListModel();

        public class GameListModel
        {
            public string SearchTerm { get; set; } = "";
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public int TotalItems { get; set; }
            public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        }

        public async Task<IActionResult> OnGetAsync(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            GameList.SearchTerm = searchTerm?.Trim() ?? "";
            GameList.PageSize = pageSize;
            GameList.PageNumber = pageNumber;

            try
            {
                Games = (await _gameService.GetGames(GameList.SearchTerm, GameList.PageNumber, GameList.PageSize)).ToList();
                GameList.TotalItems = await _gameService.GetTotalGamesCount(GameList.SearchTerm);

                if (!Games.Any() && !string.IsNullOrEmpty(GameList.SearchTerm))
                {
                    TempData["InfoMessage"] = $"No games found matching '{GameList.SearchTerm}'.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading games: {ex.Message}";
                Games = new List<GameModel>();
                GameList.TotalItems = 0;
            }

            return Page();
        }
    }
}