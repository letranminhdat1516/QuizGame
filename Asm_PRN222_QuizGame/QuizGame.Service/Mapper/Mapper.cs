using AutoMapper;
using QuizGame.Repository.Models;
using QuizGame.Service.BusinessModel;


namespace QuizGame.Service.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Question, QuestionModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Quiz, QuizModel>().ReverseMap();
        }
    }
}
