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
                cfg.CreateMap<RequestCreateDto, Request>();
                cfg.CreateMap<Organization, OrganizationDto>();
                cfg.CreateMap<OrganizationDto, Organization>()
                    .ForMember(d => d.PhoneNumbers, (options) => options.Ignore())
                    .ForMember(d => d.ActivityTypes, (options) => options.Ignore());
                cfg.CreateMap<ActivityType, ActivityTypeDto>();
                cfg.CreateMap<ActivityTypeDto, ActivityType>();
                cfg.CreateMap<Request, RequestDto>()
                    .ForMember(src => src.Owner, opt => opt.MapFrom(c => c.Organization.Name));
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