using System.ComponentModel.DataAnnotations;

namespace QuizGame.Service.BusinessModel
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }

        [Required]
        public int? QuizId { get; set; }

        [Required, StringLength(255)]
        public string QuizName { get; set; }    

        [Required, StringLength(255)]
        public string QuestionText { get; set; }

        [Required, StringLength(255)]
        public string CorrectAnswer { get; set; }

        [Required, StringLength(255)]
        public string Option1 { get; set; }

        [Required, StringLength(255)]
        public string Option2 { get; set; }

        [Required, StringLength(255)]
        public string Option3 { get; set; }

        [Required, StringLength(255)]
        public string Option4 { get; set; }

        [Range(5, 120)]
        public int? TimeLimit { get; set; } = 30;

        [Required]
        public string Status { get; set; }
    }
}
