using AutoMapper;
using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;


namespace QuizGame.Service.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Question, QuestionModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Quiz, QuizModel>().ReverseMap();
            CreateMap<GameLog, GameLogModel>().ReverseMap();
            CreateMap<QuestionInGame, QuestionInGameModel>().ReverseMap();
            CreateMap<TeamScore, ScoreModel>().ReverseMap();
            CreateMap<Game, GameModel>().ReverseMap();
        }
    }
}
