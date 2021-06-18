namespace Volunteers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// Base service
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TDto">TDto</typeparam>
    public abstract class BaseService<TEntity, TDto>
       where TEntity : BaseEntity
       where TDto : BaseDto
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="mapper">Mapper</param>
        /// <param name="repository">Repository</param>
        /// <param name="validator">validator</param>
        protected BaseService(IVolunteerMapper mapper, IDbRepository<TEntity> repository, IDtoValidator validator)
        {
            Mapper = mapper;
            Repository = repository;
            Validator = validator;
        }

        /// <summary>
        /// Validator
        /// </summary>
        protected IDtoValidator Validator { get; }

        /// <summary>
        /// Mapper.
        /// </summary>
        protected IVolunteerMapper Mapper { get; }

        /// <summary>
        /// Repository.
        /// </summary>
        protected IDbRepository<TEntity> Repository { get; }

        /// <summary>
        /// Асинхронно обновляет объект
        /// </summary>
        /// <param name="dto">Dto.</param>
        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            _ = dto ?? throw new ArgumentException(nameof(dto));
            await Validator.ValidateAndThrowAsync(dto);
            var entity =
                await Repository.Get(x => x.Id == dto.Id).FirstOrDefaultAsync();
            Mapper.Map(dto, entity);
            await Repository.UpdateAsync(entity);
            return await GetById(entity.Id);
        }

        /// <summary>
        /// Добавляет новый объект.
        /// </summary>
        /// <param name="dto">Dto.</param>
        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            _ = dto ?? throw new ArgumentException("Должен быть задан добавляемый объект");
            await Validator.ValidateAndThrowAsync(dto);
            var entity = Mapper.Map<TEntity>(dto);
            await Repository.AddAsync(entity);
            var map = await GetById(entity.Id);
            return map;
        }

        /// <summary>
        /// Добавляет новые объекты.
        /// </summary>
        /// <param name="dtos">Dtos.</param>
        public virtual async Task<List<TDto>> AddAsync(IList<TDto> dtos)
        {
            _ = dtos ?? throw new ArgumentException("Необходимо задать список добавляемых объектов");
            var result = new List<TDto>();
            foreach (var dto in dtos)
            {
                result.Add(await AddAsync(dto));
            }

            return result;
        }

        /// <summary>
        /// Удаление объекта.
        /// </summary>
        /// <param name="id">ID объекта.</param>
        public virtual async Task DeleteAsync(long id)
        {
            var entity = await Repository.Get(x => x.Id == id).FirstOrDefaultAsync();
            await Repository.DeleteAsync(entity);
        }

        /// <summary>
        /// Возвращает объект по ID.
        /// </summary>
        /// <param name="id">ID объекта.</param>
        protected virtual async Task<TDto> GetById(long id)
        {
            var result = await Repository
                .Get(x => x.Id == id)
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Возвращает список объектов
        /// </summary>
        protected virtual async Task<List<TDto>> GetAsync()
        {
            var result = await Repository
                .Get()
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return result;
        }
    }
}
