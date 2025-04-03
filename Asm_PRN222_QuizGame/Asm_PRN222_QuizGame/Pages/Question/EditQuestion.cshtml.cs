using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizGame.Service.Interface;
using QuizGame.Service.BusinessModel;
using System.Threading.Tasks;

namespace Asm_PRN222_QuizGame.Admin.Pages.Question
{
    public class EditQuestionModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public EditQuestionModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [BindProperty]
        public QuestionModel Question { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _questionService.UpdateQuestion(Question);
                SuccessMessage = "Question updated successfully!";
                return Page(); // Hiển thị thông báo thành công trên cùng trang
                // Hoặc: return RedirectToPage("./QuestionIndex"); để chuyển hướng về danh sách
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating question: {ex.Message}";
                return Page();
            }
        }
    }
}