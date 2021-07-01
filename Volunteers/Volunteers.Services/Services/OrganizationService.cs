namespace Volunteers.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// OrganizationService.
    /// </summary>
    public class OrganizationService : BaseService<Organization, OrganizationDto>
    {
        private readonly RequestService _requestService;
        private readonly IDbRepository<Request> _requestRepo;
        private readonly IDbRepository<User> _userRepo;

        /// <summary>
        /// OrganizationService.
        /// </summary>
        /// <param name="mapper">mapper.</param>
        /// <param name="repository">repository.</param>
        /// <param name="validator">validator</param>
        /// <param name="requestRepo">requestRepo</param>
        /// <param name="userRepo">userRepo</param>
        /// <param name="requestService">requestService</param>
        public OrganizationService(
            IVolunteerMapper mapper,
            IDbRepository<Organization> repository,
            IDtoValidator validator,
            IDbRepository<Request> requestRepo,
            IDbRepository<User> userRepo,
            RequestService requestService)
            : base(mapper, repository, validator)
        {
            _requestRepo = requestRepo;
            _userRepo = userRepo;
            _requestService = requestService;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="orgDto">org.</param>
        public async Task<ActionResult<OrganizationDto>> Create(OrganizationDto orgDto)
        {
            var org = Mapper.Map<Organization>(orgDto);
            await Repository.AddAsync(org);
            orgDto.Id = await Repository.Get(s => s.Mail == org.Mail).Select(x => x.Id).FirstOrDefaultAsync();
            return orgDto;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="orgDto">org.</param>
        /// <param name="id">user organizationId</param>
        public async Task<ActionResult<OrganizationDto>> Create(OrganizationDto orgDto, long id)
        {
            var org = Mapper.Map<Organization>(orgDto);
            org.UserId = id;
            await Repository.AddAsync(org);
            orgDto.UserId = id;
            orgDto.Id = await Repository.Get(s => s.Mail == orgDto.Mail).Select(x => x.Id).FirstOrDefaultAsync();
            return orgDto;
        }

        /// <summary>
        /// ChangeLogo
        /// </summary>
        /// <param name="logo">logo</param>
        public async Task<ActionResult<OrganizationDto>> ChangeLogo(OrganizationLogoDto logo)
        {
            var org = await Repository
                .Get()
                .Include(x => x.ActivityTypes)
                .Include(x => x.PhoneNumbers)
                .Where(x => x.Id == logo.Id)
                .FirstOrDefaultAsync();
            org.Logo = logo.Logo;
            await Repository.UpdateAsync(org);
            var orgDto = Mapper.Map<OrganizationDto>(org);
            return orgDto;
        }

        /// <summary>
        /// Выдача организаций по типы их активности
        /// </summary>
        /// <param name="ids">organizationId активность</param>
        public async Task<ActionResult<List<OrganizationDto>>> GetByIds(List<long> ids)
        {
            var organizationDtos = await Repository
                .Get()
                .Where(x => x.ActivityTypes.Any(at => ids.Contains(at.Id)))
                .ProjectTo<OrganizationDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return organizationDtos;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="organizationId">organizationId</param>
        public async Task<ActionResult<Organization>> Delete(long organizationId)
        {
            // находим заявки организации
            var allRequests = await _requestRepo
                .Get(x => x.OrganizationId == organizationId &&
                          x.RequestStatus == RequestStatus.Execution)
                .ToListAsync();

            // меняем статус
            foreach (var request in allRequests)
            {
                await _requestService.ChangeStatus(new RequestChangeStatusDto
                {
                    RequestId = request.Id,
                    RequestStatus = RequestStatus.Waiting
                });
            }

            // удаляем организацию
            var org = await Repository
                .Get()
                .FirstOrDefaultAsync(x => x.Id == organizationId);
            org.UserId = null;

            // получает пользователя по компании
            var userId = await Repository.Get().Where(x => x.Id == organizationId).Select(t => t.UserId).FirstOrDefaultAsync();
            await DeleteAsync(organizationId);
            await _userRepo.DeleteAsync((long)userId);
            return org;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="orgDto">orgDto</param>
        public async Task<ActionResult<OrganizationDto>> Update(OrganizationDto orgDto)
        {
            var org = await Repository
                .Get(o => o.Id == orgDto.Id)
                .Include(p => p.PhoneNumbers)
                .Include(a => a.ActivityTypes)
                .FirstOrDefaultAsync();
            Mapper.Map(orgDto, org);
            await Repository.UpdateAsync(org);
            return orgDto;
        }
    }
}
