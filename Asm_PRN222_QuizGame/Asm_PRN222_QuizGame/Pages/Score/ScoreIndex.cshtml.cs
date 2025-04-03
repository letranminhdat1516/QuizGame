using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using QuizGame.Service.BusinessModel;
using QuizGame.Service.Interface;


namespace Asm_PRN222_QuizGame.Admin.Pages.Score
{
    public class ScoreIndexModel : PageModel
    {
        private readonly IScoreService _scoreService;

        public ScoreIndexModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        // Đảm bảo Scores là thuộc tính của ScoreIndexModel, không phải ScoreListModel
        public IList<ScoreModel> Scores { get; set; } = new List<ScoreModel>();

        public ScoreListModel ScoreList { get; set; } = new ScoreListModel();

        public class ScoreListModel
        {
            public string SearchTerm { get; set; } = "";
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public int TotalItems { get; set; }
            public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        }

        public async Task<IActionResult> OnGetAsync(string searchTerm = "", int pageNumber = 1, int pageSize = 6)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 6;

            ScoreList.SearchTerm = searchTerm?.Trim() ?? "";
            ScoreList.PageSize = pageSize;
            ScoreList.PageNumber = pageNumber;

            try
            {
                Scores = (await _scoreService.GetScores(ScoreList.SearchTerm, pageNumber, pageSize)).ToList();
                ScoreList.TotalItems = await _scoreService.GetTotalScoresCount(ScoreList.SearchTerm);

                if (!Scores.Any() && !string.IsNullOrEmpty(ScoreList.SearchTerm))
                {
                    TempData["InfoMessage"] = $"No scores found matching '{ScoreList.SearchTerm}'.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading scores: {ex.Message}";
                Scores = new List<ScoreModel>();
                ScoreList.TotalItems = 0;
            }

            return Page();
        }

    }
}