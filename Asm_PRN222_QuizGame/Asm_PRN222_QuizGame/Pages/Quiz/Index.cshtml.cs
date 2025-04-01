using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Asm_PRN222_QuizGame.Admin.Pages.Quiz
{
    public class IndexModel : PageModel
    {
        private readonly IQuizService _quizService;

        public IndexModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public IList<QuizModel> Quizs { get; set; } = new List<QuizModel>();

        public QuizListModel QuizList { get; set; } = new QuizListModel();

        public class QuizListModel
        {
            public string SearchTerm { get; set; } = "";
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public int TotalItems { get; set; }
            public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        }

        public async Task<IActionResult> OnGetAsync(string searchTerm = "", int pageNumber = 1, int pageSize = 6)
        {
            // Đảm bảo số trang hợp lệ
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 6;

            QuizList.SearchTerm = searchTerm;
            QuizList.PageSize = pageSize;
            QuizList.PageNumber = pageNumber;

            // Lấy danh sách quiz có phân trang
            Quizs = (await _quizService.GetQuizs(searchTerm, pageNumber, pageSize)).ToList();

            // Lấy tổng số quiz để phân trang
            QuizList.TotalItems = await _quizService.GetTotalQuizCount(searchTerm);

            return Page();
        }
    }
}
