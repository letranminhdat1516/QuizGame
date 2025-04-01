using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Asm_PRN222_QuizGame.Admin.Pages.Question
{
    public class QuestionIndexModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public QuestionIndexModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public IList<QuestionModel> Questions { get; set; } = new List<QuestionModel>();

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

           
                Questions = (IList<QuestionModel>)(await _questionService.GetQuestions(searchTerm, pageNumber, pageSize)).ToList();
              
                QuestionList.TotalItems = await _questionService.GetTotalQuestionsCount(searchTerm);

               
            
          

            return Page();
        }
    }
}