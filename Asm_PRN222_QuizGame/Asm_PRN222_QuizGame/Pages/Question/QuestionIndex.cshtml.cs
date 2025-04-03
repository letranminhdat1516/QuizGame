using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asm_PRN222_QuizGame.Admin.Pages.Question
{
    public class QuestionIndexModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IQuizService _quizService; // Inject Quiz Service

        public QuestionIndexModel(IQuestionService questionService, IQuizService quizService)
        {
            _questionService = questionService;
            _quizService = quizService; // Initialize the quiz service
        }

        public IList<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        public List<SelectListItem> Quizzes { get; set; } = new List<SelectListItem>();
        public QuestionListModel QuestionList { get; set; } = new QuestionListModel();

        public class QuestionListModel
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

            QuestionList.SearchTerm = searchTerm;
            QuestionList.PageSize = pageSize;
            QuestionList.PageNumber = pageNumber;

            try
            {
                // Fetch questions
                Questions = (await _questionService.GetQuestions(searchTerm, pageNumber, pageSize)).ToList();

                // Fetch total number of questions for pagination
                QuestionList.TotalItems = await _questionService.GetTotalQuestionsCount(searchTerm);

                // Fetch quizzes for dropdown list (if needed)
                await LoadQuizzes();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading questions: {ex.Message}";
                Questions = new List<QuestionModel>();
                QuestionList.TotalItems = 0;
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
