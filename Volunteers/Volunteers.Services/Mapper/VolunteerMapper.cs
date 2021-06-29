
namespace Volunteers.Services.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
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
                cfg.CreateMap<Request, RequestDto>()
                    .ForMember(src => src.Owner, opt => opt.MapFrom(c => c.Organization.Name)); 
                cfg.CreateMap<OrganizationDto, Organization>()
                    .ForMember(x => x.ActivityTypes, opt => opt.Ignore())
                    .ForMember(x => x.PhoneNumbers, opt => opt.Ignore())
                    .AfterMap(MapPhones)
                    .AfterMap(MapActivityTypes);
                cfg.CreateMap<Organization, OrganizationDto>()
                    .ForMember(x => x.ActivityTypes, opt => opt.MapFrom(x => x.ActivityTypes.Where(at => !at.IsDeleted)))
                    .ForMember(x => x.PhoneNumbers, opt => opt.MapFrom(x => x.PhoneNumbers.Select(pn => pn.PhoneNumber)));
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

        private void MapPhones(OrganizationDto dto, Organization entity)
        {
            var entityPhoneNumbers = entity.PhoneNumbers.Select(x => x.PhoneNumber).ToList();
            var newPhoneNumbers = dto.PhoneNumbers.Except(entityPhoneNumbers).ToList();
            var deletedPhones = entityPhoneNumbers.Except(dto.PhoneNumbers).ToList();

            newPhoneNumbers.ForEach(x => entity.PhoneNumbers.Add(new Phone { PhoneNumber = x, OrganizationId = entity.Id }));
            deletedPhones.ForEach(deletedPhone =>
                entity.PhoneNumbers.Remove(entity.PhoneNumbers.First(phone =>
                    phone.PhoneNumber.Equals(deletedPhone))));
        }

        private void MapActivityTypes(OrganizationDto dto, Organization entity)
        {
            var entityActivities = entity.ActivityTypes.Select(x => x.Id).ToList();
            var newActivities = dto.ActivityTypes.Select(x => x.Id).Except(entityActivities).ToList();
            var deletedActivities = entityActivities.Except(dto.ActivityTypes.Select(x => x.Id)).ToList();

            newActivities.ForEach(x => entity.ActivityTypeOrganizations
                .Add(new ActivityTypeOrganization { ActivityTypeId = x, OrganizationId = entity.Id }));
            deletedActivities.ForEach(deletedActivities =>
                entity.ActivityTypeOrganizations.Remove(entity.ActivityTypeOrganizations.First(act =>
                    act.ActivityTypeId.Equals(deletedActivities))));
        }
    }
}