using QuizGame.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public int? QuizId { get; set; }
        public string QuestionText { get; set; }
<<<<<<< HEAD

        public char CorrectAnswer { get; set; }
=======
        public string CorrectAnswer { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
>>>>>>> origin/NguyenHP

    }
}
