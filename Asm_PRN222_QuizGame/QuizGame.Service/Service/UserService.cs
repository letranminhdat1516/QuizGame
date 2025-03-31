using AutoMapper;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;
using QuizGame.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddUser(UserModel user)
        {
            try
            {
                var userRepo = _unitOfWork.GetRepository<User>();
                var user_temp = _mapper.Map<User>(user);
                await userRepo.AddAsync(user_temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserModel> GetUserById(int id)
        {
            try
            {
                var userRepo = _unitOfWork.GetRepository<User>();
                var user_temp = await userRepo.GetByIdAsync(id);          
                return _mapper.Map<UserModel>(user_temp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUser(UserModel user)
        {
            try
            {
                var userRepo = _unitOfWork.GetRepository<User>();
                var user_temp = _mapper.Map<User>(user);
                await userRepo.UpdateAsync(user_temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserModel> Login(string email, string password)
        {
            var userRepo =  _unitOfWork.GetRepository<User>();
            var user_temp = await userRepo.FindAsync(a => a.Email == email && a.Password == password);
            var user = user_temp.FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserModel>(user);
        }
    }
}
