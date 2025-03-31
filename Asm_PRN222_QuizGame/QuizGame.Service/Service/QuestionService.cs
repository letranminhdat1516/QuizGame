using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddQuestion(QuestionModel question)
        {
            try
            {
                var question_Temp = _mapper.Map<Question>(question);
                var questionRepository = _unitOfWork.GetRepository<Question>();
                await questionRepository.AddAsync(question_Temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding question: {ex.Message}", ex);
            }
        }

        public async Task<bool> CheckStatus(int id)
        {
            try
            {
                var questionRepository = _unitOfWork.GetRepository<Question>();
                var question = await questionRepository.GetByIdAsync(id);

                // Assuming you have a Status property in the Question model
                // If not, you'll need to modify this based on your actual model
                return question != null; // Replace with actual status check
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking question status: {ex.Message}", ex);
            }
        }

        public async Task<QuestionModel> GetQuestionById(int id)
        {
            try
            {
                var questionRepository = _unitOfWork.GetRepository<Question>();
                var question_Temp = await questionRepository.GetByIdAsync(id);
                return _mapper.Map<QuestionModel>(question_Temp);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving question: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<QuestionModel>> GetQuestions(string search, int pageNumber, int pageSize)
        {
            try
            {
                var questionRepository = _unitOfWork.GetRepository<Question>();

                var query = questionRepository.AsQueryable();


                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(q => q.QuestionText.Contains(search) ||
                                             q.CorrectAnswer.Contains(search));
                }

                // Calculate pagination
                int skip = (pageNumber - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);
                var questions = await query.ToListAsync();

                // Map to QuestionModel and return
                return _mapper.Map<IEnumerable<QuestionModel>>(questions);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving questions: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<QuestionModel>> GetQuestionsWithQuizId(int? quizId)
        {
            try
            {
                if (quizId == null)
                {
                    return Enumerable.Empty<QuestionModel>();
                }
                var questionRepository = _unitOfWork.GetRepository<Question>();

                var query = questionRepository.AsQueryable();

                query = query.Where(q => q.QuizId == quizId);
                var questions = await query.ToListAsync();
                // Map to QuestionModel and return
                return _mapper.Map<IEnumerable<QuestionModel>>(questions);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving questions: {ex.Message}", ex);
            }
        }
        public async Task RemoveQuestion(int id)
        {
            try
            {
                var questionRepository = _unitOfWork.GetRepository<Question>();
                await questionRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing question: {ex.Message}", ex);
            }
        }

        public async Task SetStatusQuestion(int id, bool status)
        {
            try
            {
                var questionRepository = _unitOfWork.GetRepository<Question>();
                var question = await questionRepository.GetByIdAsync(id);

                if (question == null)
                {
                    throw new ArgumentException("Question not found");
                }

                // Assuming you have a Status property in the Question model
                // If not, you'll need to modify this based on your actual model
                // question.Status = status;

                await questionRepository.UpdateAsync(question);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error setting question status: {ex.Message}", ex);
            }
        }

        public async Task UpdateQuestion(QuestionModel question)
        {
            try
            {
                var questionRepository = _unitOfWork.GetRepository<Question>();
                var quetion_temp = _mapper.Map<Question>(question);
                await questionRepository.UpdateAsync(quetion_temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating question: {ex.Message}", ex);
            }
        }

        public async Task<Game> GetGameByPinCode(string pinCode)
        {
            var gameRepository = _unitOfWork.GetRepository<Game>();
            var game = await gameRepository.AsQueryable()
                .FirstOrDefaultAsync(g => g.GamePin == pinCode && g.Status == "Waiting");

            return game;
        }

        public async Task<QuestionModel> GetNextQuestionForGame(int gameId, int questionNumber)
        {
            var questionInGameRepository = _unitOfWork.GetRepository<QuestionInGame>();
            var questionInGame = await questionInGameRepository.AsQueryable()
                .FirstOrDefaultAsync(qig => qig.GameId == gameId && qig.QuestionNumber == questionNumber); // Lấy câu hỏi tiếp theo (giả sử là câu hỏi đầu tiên)

            if (questionInGame == null)
            {
                return null;  // Nếu không còn câu hỏi, trả về null
            }

            var questionRepository = _unitOfWork.GetRepository<Question>();
            var question = await questionRepository.GetByIdAsync(questionInGame.QuestionId.GetValueOrDefault());

            return new QuestionModel
            {
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText,
                CorrectAnswer = question.CorrectAnswer
            };
        }

        public async Task AddQuestionInGame(QuestionInGameModel questionInGameModel)
        {
            try
            {
                var question_Temp = _mapper.Map<QuestionInGame>(questionInGameModel); 
                var questionRepository = _unitOfWork.GetRepository<QuestionInGame>();
                await questionRepository.AddAsync(question_Temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding question: {ex.Message}", ex);
            }
        }
    }
}