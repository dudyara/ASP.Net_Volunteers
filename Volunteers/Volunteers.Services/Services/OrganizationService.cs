namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// OrganizationService.
    /// </summary>
    public class OrganizationService : BaseService<Organization>
    {

        /// <summary>
        /// OrganizationService.
        /// </summary>
        /// <param name="mapper">mapper.</param>
        /// <param name="repository">repository.</param>
        /// <param name="validator">validator</param>
        public OrganizationService(
            IVolunteerMapper mapper,
            IDbRepository<Organization> repository,
            IValidator validator)
            : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns>.</returns>
        public async Task<ActionResult<IEnumerable<List<OrganizationDto>>>> Get()
        {
            var orgs = await Repository
                .Get()
                .ProjectTo<List<OrganizationDto>>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return orgs;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="orgDto">org.</param>
        public async Task<ActionResult<Organization>> Create(OrganizationDto orgDto)
        {
            var context = new ValidationContext<OrganizationDto>(orgDto);
            var validateResult = Validator.Validate(context);
            if (validateResult.IsValid == false)
                throw new Exception("Неверный формат данных");

            var org = Mapper.Map<Organization>(orgDto);
            for (int i = 0; i < orgDto.Phones.Count; i++)
                org.PhoneNumbers.Add(new PhoneNumber() { Phone = orgDto.Phones[i] });
            for (int i = 0; i < orgDto.KeyWords.Count; i++)
                org.ActivityTypeOrganizations.Add(new ActivityTypeOrganization() { ActivityTypeId = orgDto.KeyWords[i].Id });
            await Repository.Add(org);
            return org;
        }

        /// <summary>
        /// Выдача организаций по типы их активности
        /// </summary>
        /// <param name="ids">id активность</param>
        /// <returns></returns>
        public async Task<ActionResult<List<OrganizationDto>>> GetByIds(List<long> ids)
        {
            var result = await Repository
                .Get()
                .Where(x => x.ActivityTypes.Any(at => ids.Contains(at.Id)))
                .ToListAsync();
            var organizationDtos = Mapper.Map<List<OrganizationDto>>(result);
            return organizationDtos;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<ActionResult<Organization>> Delete(long id)
        {
            var org = await Repository
                .Get()
                .FirstOrDefaultAsync(x => x.Id == id);
            await Repository.Delete(org);
            return org;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="orgDto">orgDto</param>
        /// <returns></returns>
        public async Task<ActionResult<Organization>> Update(OrganizationDto orgDto)
        {
            var context = new ValidationContext<OrganizationDto>(orgDto);
            var validateResult = Validator.Validate(context);
            if (validateResult.IsValid == false)
                throw new Exception("Неверный формат данных");

            var org = Mapper.Map<Organization>(orgDto);
            for (int i = 0; i < orgDto.Phones.Count; i++)
                org.PhoneNumbers.Add(new PhoneNumber() { Id = orgDto.Id, Phone = orgDto.Phones[i] });
            for (int i = 0; i < orgDto.KeyWords.Count; i++)
            {
                org.ActivityTypeOrganizations.Add(new ActivityTypeOrganization()
                {
                    OrganizationId = orgDto.Id,
                    ActivityTypeId = orgDto.KeyWords[i].Id
                });
            }

            await Repository.Update(org);
            return org;
        }
    }
}
