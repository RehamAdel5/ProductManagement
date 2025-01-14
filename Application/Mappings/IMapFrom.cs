using AutoMapper;

namespace Application.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
