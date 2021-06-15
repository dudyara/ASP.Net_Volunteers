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
    public class OrganizationService : BaseService<Organization, OrganizationDto>
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
        public async Task<ActionResult<List<OrganizationDto>>> Get()
        {
            var orgs = await Repository
                .Get()
                .Include(d => d.ActivityTypes)
                .Include(c => c.PhoneNumbers)
                .ToListAsync();
            var orgDto = Mapper.Map<List<OrganizationDto>>(orgs);
            return orgDto;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="orgDto">org.</param>
        /// <param name="id">id</param>
        public async Task<ActionResult<OrganizationDto>> Create(OrganizationDto orgDto)
        {
            var context = new ValidationContext<OrganizationDto>(orgDto);
            var validateResult = Validator.Validate(context);
            if (validateResult.IsValid == false)
                throw new Exception("Неверный формат данных");

            var org = Mapper.Map<Organization>(orgDto);
            for (int i = 0; i < orgDto.PhoneNumbers.Count; i++)
                org.PhoneNumbers.Add(new Phone() { PhoneNumber = orgDto.PhoneNumbers[i] });
            for (int i = 0; i < orgDto.ActivityTypes.Count; i++)
                org.ActivityTypeOrganizations.Add(new ActivityTypeOrganization() { ActivityTypeId = orgDto.ActivityTypes[i].Id });
            await Repository.Add(org);
            return orgDto;
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
            await DeleteAsync(id);
            return org;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="orgDto">orgDto</param>
        /// <returns></returns>
        public async Task<ActionResult<OrganizationDto>> Update(OrganizationDto orgDto)
        {
            var context = new ValidationContext<OrganizationDto>(orgDto);
            var validateResult = Validator.Validate(context);
            if (validateResult.IsValid == false)
                throw new Exception("Неверный формат данных");

            var org = Mapper.Map<Organization>(orgDto);
            for (int i = 0; i < orgDto.PhoneNumbers.Count; i++)
                org.PhoneNumbers.Add(new Phone() { Id = orgDto.Id, PhoneNumber = orgDto.PhoneNumbers[i] });
            for (int i = 0; i < orgDto.ActivityTypes.Count; i++)
            {
                org.ActivityTypeOrganizations.Add(new ActivityTypeOrganization()
                {
                    OrganizationId = orgDto.Id,
                    ActivityTypeId = orgDto.ActivityTypes[i].Id
                });
            }

            await Repository.Update(org);
            return orgDto;
        }
    }
}
