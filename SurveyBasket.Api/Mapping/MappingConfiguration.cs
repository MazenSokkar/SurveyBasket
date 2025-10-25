using Mapster;
using SurveyBasket.Contracts;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Core.Entities;

namespace SurveyBasket.Api.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer {Content = answer}));
        }
    }
}
