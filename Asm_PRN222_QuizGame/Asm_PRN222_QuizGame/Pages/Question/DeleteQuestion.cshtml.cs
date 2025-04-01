using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;

namespace Asm_PRN222_QuizGame.Admin.Pages.Question
{
    public class DeleteQuestionModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public DeleteQuestionModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [BindProperty]
        public QuestionModel Question { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Question = await _questionService.GetQuestionById(id.Value);
                if (Question == null)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _questionService.RemoveQuestion(id.Value);
                return RedirectToPage("./QuestionIndex");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting question: {ex.Message}";
                return Page();
            }
        }
    }
}