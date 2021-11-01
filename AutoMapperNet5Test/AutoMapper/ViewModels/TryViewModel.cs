namespace AutoMapperNet5Test.AutoMapper.ViewModels
{
    using AutoMapperNet5Test.AutoMapper.Models;
    using global::AutoMapper;

    public class TryViewModel
        : IMapFrom<TryModel>,
            IMapExplicitly
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public void RegisterMappings(IProfileExpression configuration)
        {
            string prefix = default;

            configuration.CreateMap<TryModel, TryViewModel>()
                .ForMember(
                    x => x.FullName,
                    opt => opt.MapFrom(
                        src => prefix + " " + src.FirstName + " " + src.LastName));
        }
    }
}