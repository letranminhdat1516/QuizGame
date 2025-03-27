using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.BusinessModel
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
