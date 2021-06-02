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
                cfg.CreateMap<CreateRequestDto, Request>();
                cfg.CreateMap<Organization, OrganizationDto>()
                   .ForMember(src => src.Manager, opt => opt.MapFrom(c => c.ChiefFIO))
                   .ForMember(src => src.KeyWords, opt => opt.MapFrom(c => c.ActivityTypes))
                   .ForMember(src => src.Phones, opt => opt.MapFrom(c => c.PhoneNumbers));
                cfg.CreateMap<OrganizationDto, Organization>();
                cfg.CreateMap<ActivityType, ActivityTypeDto>()
                    .ForMember(src => src.Label, opt => opt.MapFrom(c => c.TypeName));
                cfg.CreateMap<Request, RequestDto>()
                    .ForMember(src => src.Owner, opt => opt.MapFrom(c => c.Organization.Name))
                    .ForMember(src => src.CreationDate, opt => opt.MapFrom(c => c.StartDate))
                    .ForMember(src => src.CompletionDate, opt => opt.MapFrom(c => c.FinishDate))
                    .ForMember(src => src.Name, opt => opt.MapFrom(c => c.FIO))
                    .ForMember(src => src.Phone, opt => opt.MapFrom(c => c.PhoneNumber));
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