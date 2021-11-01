namespace AutoMapperNet5Test.AutoMapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::AutoMapper;
    using global::AutoMapper.QueryableExtensions;

    public static class AutoMapperExtensions
    {
        private static readonly Type MapFromType = typeof(IMapFrom<>);
        private static readonly Type MapToType = typeof(IMapTo<>);
        private static readonly Type ExplicitMapType = typeof(IMapExplicitly);

        public static void RegisterMappingsFrom(
            this IMapperConfigurationExpression mapper,
            params string[] assemblyNames)
            => assemblyNames
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    AllMapFrom = GetMappingModels(t, MapFromType),
                    AllMapTo = GetMappingModels(t, MapToType),
                    ExplicitMap = t
                        .GetInterfaces()
                        .Where(i => ExplicitMapType.IsAssignableFrom(i))
                        .Select(i => (IMapExplicitly)Activator.CreateInstance(t))
                        .FirstOrDefault(),
                })
                .ToList()
                .ForEach(t =>
                {
                    t.AllMapFrom
                        .ToList()
                        .ForEach(mapFrom => mapper.CreateMap(mapFrom, t.Type));

                    t.AllMapTo
                        .ToList()
                        .ForEach(mapTo => mapper.CreateMap(t.Type, mapTo));

                    t.ExplicitMap?.RegisterMappings(mapper);
                });

        private static IEnumerable<Type> GetMappingModels(Type source, Type mappingType)
            => source
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == mappingType)
                .Select(i => i.GetGenericArguments().First());

        public static IEnumerable<Type> GetExportedTypes(this string[] assemblyNames)
            => assemblyNames
                .Select(a =>
                {
                    try
                    {
                        return Assembly.Load(new AssemblyName(a));
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(assembly => assembly != null)
                .SelectMany(a => a.ExportedTypes);

        public static TDestination Map<TDestination>(this object item)
            => AutoMapperSingleton.Instance.Mapper.Map<TDestination>(item);

        public static IQueryable<TDestination> MapCollection<TDestination>(this IQueryable queryable,
            object parameters = null)
            => AutoMapperSingleton.Instance.Mapper.ProjectTo<TDestination>(queryable, parameters);
    }
}