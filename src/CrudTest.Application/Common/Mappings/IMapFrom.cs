using AutoMapper;

namespace CrudTest.Application.Common.Mappings
{
    /**
    * This interface is provided so DTOs can have their mapping implementations
    * within themselves.
    */
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}