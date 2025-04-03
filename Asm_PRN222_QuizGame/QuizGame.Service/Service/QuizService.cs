using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class QuizService : IQuizService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuizService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddQuiz(QuizModel quiz)
        {
            try
            {
                var quiz_Temp = _mapper.Map<Quiz>(quiz);
                var quizRepository = _unitOfWork.GetRepository<Quiz>();
                await quizRepository.AddAsync(quiz_Temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Quiz: {ex.Message}", ex);
            }
        }

        public async Task<QuizModel> GetQuizById(int id)
        {
            try
            {
                var quizRepository = _unitOfWork.GetRepository<Quiz>();
                var questionRepository = _unitOfWork.GetRepository<Question>();

                var quiz_Temp = await quizRepository.GetByIdAsync(id);
                if (quiz_Temp == null)
                    return null;

                var questions = await questionRepository.AsQueryable()
                    .Where(q => q.QuizId == id)
                    .Select(q => new QuestionModel
                    {
                        QuestionId = q.QuestionId,
                        QuestionText = q.QuestionText,
                        CorrectAnswer = q.CorrectAnswer
                    })
                    .ToListAsync();

                var quizModel = _mapper.Map<QuizModel>(quiz_Temp);
                quizModel.Questions = questions; // ✅ Gán danh sách câu hỏi vào model

                return quizModel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Quiz: {ex.Message}", ex);
            }
        }



        public async Task<IEnumerable<QuizModel>> GetQuizs(string search, int pageNumber, int pageSize)
        {
            try
            {
                var quizRepository = _unitOfWork.GetRepository<Quiz>();

                var query = quizRepository.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(q => EF.Functions.Like(q.QuizName, $"%{search}%"));
                }

                // Sắp xếp theo ID để đảm bảo kết quả đúng thứ tự
                query = query.OrderBy(q => q.QuizId);

                // Áp dụng phân trang
                int skip = (pageNumber - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);

                var quizs = await query.ToListAsync();
                return _mapper.Map<IEnumerable<QuizModel>>(quizs);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Quizs: {ex.Message}", ex);
            }
        }


        public async Task RemoveQuiz(int id)
        {
            try
            {
                var quizRepository = _unitOfWork.GetRepository<Quiz>();
                await quizRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing Quiz: {ex.Message}", ex);
            }
        }

        public async Task UpdateQuiz(QuizModel Quiz)
        {
            try
            {
                var quizRepository = _unitOfWork.GetRepository<Quiz>();
                var quetion_temp = _mapper.Map<Quiz>(Quiz);
                await quizRepository.UpdateAsync(quetion_temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating Quiz: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalQuizCount(string searchTerm)
        {
            var quizRepository = _unitOfWork.GetRepository<Quiz>();
            var query = quizRepository.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(q => q.QuizName.Contains(searchTerm));
            }

            return await query.CountAsync();
        

        }
    }
}
