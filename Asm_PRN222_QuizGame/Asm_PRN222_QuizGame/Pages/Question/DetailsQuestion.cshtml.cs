using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.Question
{
    public class DetailsModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public DetailsModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

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
                ErrorMessage = $"Error loading question details: {ex.Message}";
            }

            return Page();
        }
    }
}