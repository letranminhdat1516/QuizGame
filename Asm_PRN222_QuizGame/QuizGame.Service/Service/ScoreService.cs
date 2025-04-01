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
    public class ScoreService : IScoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ScoreModel> GetScoreById(int id)
        {
            try
            {
                var scoreRepository = _unitOfWork.GetRepository<TeamScore>();
                var score = await scoreRepository.GetByIdAsync(id);
                if (score == null)
                {
                    throw new Exception($"Score with ID {id} not found.");
                }
                return _mapper.Map<ScoreModel>(score);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving score: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ScoreModel>> GetScores(string search, int pageNumber, int pageSize)
        {
            try
            {
                var scoreRepository = _unitOfWork.GetRepository<TeamScore>();
                var query = scoreRepository.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    // Tìm kiếm dựa trên TeamId hoặc QuestionInGameId
                    query = query.Where(s => s.Score.ToString().Contains(search) ||
                                             s.Rank.ToString().Contains(search));
                }

                query = query.OrderBy(s => s.TeamScoreId);
                int skip = (pageNumber - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);

                var scores = await query.ToListAsync();
                return _mapper.Map<IEnumerable<ScoreModel>>(scores);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving scores: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalScoresCount(string search)
        {
            try
            {
                var scoreRepository = _unitOfWork.GetRepository<TeamScore>();
                var query = scoreRepository.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(s => s.Score.ToString().Contains(search) ||
                                             s.Rank.ToString().Contains(search));
                }

                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error counting scores: {ex.Message}", ex);
            }
        }

        public async Task AddScore(ScoreModel score)
        {
            try
            {
                var scoreEntity = _mapper.Map<TeamScore>(score);
                var scoreRepository = _unitOfWork.GetRepository<TeamScore>();
                await scoreRepository.AddAsync(scoreEntity);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding score: {ex.Message}", ex);
            }
        }

        public async Task UpdateScore(ScoreModel score)
        {
            try
            {
                var scoreRepository = _unitOfWork.GetRepository<TeamScore>();
                var existingScore = await scoreRepository.GetByIdAsync(score.TeamScoreId);
                if (existingScore == null)
                {
                    throw new Exception($"Score with ID {score.TeamScoreId} not found.");
                }

                _mapper.Map(score, existingScore);
                await scoreRepository.UpdateAsync(existingScore);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating score: {ex.Message}", ex);
            }
        }

        public async Task RemoveScore(int id)
        {
            try
            {
                var scoreRepository = _unitOfWork.GetRepository<TeamScore>();
                var score = await scoreRepository.GetByIdAsync(id);
                if (score == null)
                {
                    throw new Exception($"Score with ID {id} not found.");
                }

                await scoreRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing score: {ex.Message}", ex);
            }
        }
        
    }
}
