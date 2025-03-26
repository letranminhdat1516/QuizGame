using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Repository.Contact
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollBack();
        void Save();
        Task SaveAsync();
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
