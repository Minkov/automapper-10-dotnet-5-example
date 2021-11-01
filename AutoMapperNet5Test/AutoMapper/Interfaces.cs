namespace AutoMapperNet5Test.AutoMapper
{
    using global::AutoMapper;

    public interface IMapFrom<TModel>
    {
    }

    public interface IMapTo<TModel>
    {
    }

    public interface IMapExplicitly
    {
        void RegisterMappings(IProfileExpression configuration);
    }
}