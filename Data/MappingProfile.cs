using AutoMapper;
using Jeopardy.Models.ViewModels;

namespace Jeopardy.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameViewModel>();   
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Answer, AnswerViewModel>();
        }
    }
}