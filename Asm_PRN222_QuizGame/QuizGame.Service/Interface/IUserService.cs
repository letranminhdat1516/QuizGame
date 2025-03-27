using QuizGame.Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Interface
{
    public interface IUserService
    {
        Task<UserModel> GetUserById(int id);
        Task AddUser(UserModel user);
        Task UpdateUser(UserModel user);
    }
}
