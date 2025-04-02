using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.ComponentModel.DataAnnotations;

namespace Asm_PRN222_QuizGame.Admin.Pages.Question
{
    public class CreateQuestionModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IQuizService _quizService;

        public CreateQuestionModel(IQuestionService questionService, IQuizService quizService)
        {
            _questionService = questionService;
            _quizService = quizService;
        }

        [BindProperty]
        public QuestionModel Question { get; set; } = new QuestionModel();

        public List<SelectListItem> QuizList { get; set; } = new List<SelectListItem>();

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var quizzes = await _quizService.GetQuizs("", 1, int.MaxValue);
            QuizList = quizzes.Select(q => new SelectListItem
            {
                Value = q.QuizId.ToString(),
                Text = q.QuizName
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(); // Load lại dropdown khi có lỗi
            }

            try
            {
                await _questionService.AddQuestion(Question);
                return RedirectToPage("./QuestionIndex");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating question: {ex.Message}";
                return await OnGetAsync();
            }
        }
    }
}
