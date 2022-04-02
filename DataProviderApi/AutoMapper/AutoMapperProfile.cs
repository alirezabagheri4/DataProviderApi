using AutoMapper;
using DataProviderApi.Tools;
using DomainModel.DataModel;

namespace DataProviderApi.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DynamicObjectDO, DynamicObject>()
                .ForPath(p => p.ClassNameAndId.Item1, m => m.MapFrom(g => g.ClassNameAndId.Item1))
                .ForPath(p => p.ClassNameAndId.Item2, m => m.MapFrom(g => g.ClassNameAndId.Item2))
                    .ForMember(p => p.PropertyName, m => m.MapFrom(g => g.PropertyName))
                        .ForMember(p => p.PropertyType, m => m.MapFrom(g => g.PropertyType))
                .ForMember(p => p.PropertyValue, m => m.MapFrom(g => g.PropertyValue))
                    .ReverseMap();
        }
    }
}
