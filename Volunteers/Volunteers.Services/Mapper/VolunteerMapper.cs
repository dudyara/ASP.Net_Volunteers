namespace Volunteers.Services.Mapper
{
    using AutoMapper;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;

    /// <summary>
    /// Базовый класс для маппера
    /// </summary>
    public class VolunteerMapper : IVolunteerMapper
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public VolunteerMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // cfg.CreateMap<Request, RequestDto>();
                cfg.CreateMap<CreateRequestDto, Request>();
                cfg.CreateMap<Organization, OrganizationDto>();
                cfg.CreateMap<OrganizationDto, Organization>();
                cfg.CreateMap<ActivityType, ActivityTypeDto>();
                cfg.CreateMap<Request, RequestDto>()
                                    .ForMember("EnglishRequestPriority", opt => opt.MapFrom(c => c.RequestPriority))
                                    .ForMember("RussianRequestPriority", opt => opt.MapFrom(c => EnumsExtension.GetDescription(c.RequestPriority)));
            });
            Mapper = config.CreateMapper();
        }

        /// <inheritdoc />
        public IConfigurationProvider ConfigurationProvider => Mapper.ConfigurationProvider;

        /// <summary>
        /// Объект маппера
        /// </summary>
        protected IMapper Mapper { get; set; }

        /// <inheritdoc />
        public T Map<T>(object source)
        {
            return Mapper.Map<T>(source);
        }

        /// <inheritdoc />
        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper.Map(source, destination);
        }
    }
}