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
    public class GameLogService : IGameLog
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddGameLog(GameLogModel gameLog)
        {
            try
            {
                var gameLog_Temp = _mapper.Map<GameLog>(gameLog);
                var gameLogRepository = _unitOfWork.GetRepository<GameLog>();
                await gameLogRepository.AddAsync(gameLog_Temp);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding gameLog: {ex.Message}", ex);
            }
        }

        public async Task<string> CheckAction(int id)
        {
            try
            {
                var gameLogRepository = _unitOfWork.GetRepository<GameLog>();
                var gameLog = await gameLogRepository.GetByIdAsync(id);
                var action = gameLog.Action;
                
                if (gameLogRepository == null)
                {
                    return null;
                }
                return action;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding gameLog: {ex.Message}", ex);
            }
        }

        public async Task<GameLogModel> GetGameLogById(int id)
        {
            try
            {
                var gameLogRepository = _unitOfWork.GetRepository<GameLog>();
                var gameLog = await gameLogRepository.GetByIdAsync(id);
                if (gameLogRepository == null)
                {
                    return null;
                }
                return _mapper.Map<GameLogModel>(gameLog);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding gameLog: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<GameLogModel>> GetGameLogs(int pageNumber, int pageSize)
        {
            try
            {
                var gameLogRepository = _unitOfWork.GetRepository<GameLog>();
                var query = gameLogRepository.AsQueryable();
                // Calculate pagination
                int skip = (pageNumber - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);
                var gameLog = await query.ToListAsync();
                // Map to QuestionModel and return
                return _mapper.Map<IEnumerable<GameLogModel>>(gameLog);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving gameLog: {ex.Message}", ex);
            }
        }

        public async Task RemoveGameLog(int id)
        {
            try
            {
                var gameLogRepository = _unitOfWork.GetRepository<GameLog>();
                await gameLogRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding gameLog: {ex.Message}", ex);
            }
        }
    }
}
