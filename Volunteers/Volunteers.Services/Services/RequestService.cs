namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;
    using Excel = Microsoft.Office.Interop.Excel;

    /// <summary>
    /// RequestService
    /// </summary>
    public class RequestService : BaseService<Request, RequestDto>
    {
        /// <summary>
        /// Метод заявок.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="repository">repository</param>
        /// <param name="validator">validator</param>
        public RequestService(IVolunteerMapper mapper, IDbRepository<Request> repository, IDtoValidator validator)
            : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// GetPull.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="orgId">id</param>
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get(RequestStatus status, long orgId)
        {
            var requestsDto = new List<RequestDto>();
            if ((status == 0) && (orgId == 0))
            {
                requestsDto = await Repository.Get().ProjectTo<RequestDto>(Mapper.ConfigurationProvider).ToListAsync();
            }
            else
            if (status == 0)
            {
                requestsDto = await Repository
                    .Get()
                    .Where(r => r.OrganizationId == orgId)
                    .ProjectTo<RequestDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            else
            if (orgId == 0)
            {
                requestsDto = await Repository
                    .Get()
                    .Where(r => r.RequestStatus == status)
                    .ProjectTo<RequestDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            else
            {
                requestsDto = await Repository
                    .Get()
                    .Where(r => r.OrganizationId == orgId)
                    .Where(r => r.RequestStatus == status)
                    .ProjectTo<RequestDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();
            }

            return requestsDto;
        }

        /// <summary>
        /// GetCount
        /// </summary>
        /// <param name="orgId">orgId.</param>
        public async Task<int[]> GetCount(long orgId)
        {
            int[] result = new int[3];
            result[0] = await Repository
                .Get()
                .Where(c => c.RequestStatus == RequestStatus.Waiting)
                .CountAsync();
            if (orgId == 0)
            {
                result[1] = await Repository
                    .Get()
                    .Where(c => c.RequestStatus == RequestStatus.Execution)
                    .CountAsync();
                result[2] = await Repository
                    .Get()
                    .Where(c => c.RequestStatus == RequestStatus.Done)
                    .CountAsync();
            }
            else
            {
                result[1] = await Repository
                    .Get()
                    .Where(c => c.OrganizationId == orgId)
                    .Where(c => c.RequestStatus == RequestStatus.Execution)
                    .CountAsync();
                result[2] = await Repository
                    .Get()
                    .Where(c => c.OrganizationId == orgId)
                    .Where(c => c.RequestStatus == RequestStatus.Done)
                    .CountAsync();
            }

            return result;
        }

        /// <summary>
        /// Изменить статус заявки
        /// </summary>
        /// <param name="reqDto">reqDto</param>
        public async Task<ActionResult<Request>> ChangeStatus(RequestChangeStatusDto reqDto)
        {
            var request = await Repository.Get(x => (x.Id == reqDto.RequestId)).FirstOrDefaultAsync();
            switch (reqDto.RequestStatus)
            {
                case RequestStatus.Waiting:
                    request.OrganizationId = null;
                    request.RequestStatus = RequestStatus.Waiting;
                    break;
                case RequestStatus.Execution:
                    if (request.RequestStatus == RequestStatus.Waiting)
                        request.OrganizationId = reqDto.OrganizationId;
                    request.RequestStatus = RequestStatus.Execution;
                    request.Completed = null;
                    break;
                case RequestStatus.Done:
                    request.RequestStatus = RequestStatus.Done;
                    request.Completed = DateTime.Now;
                    break;
            }

            await Repository.UpdateAsync(request);
            return request;
        }

        /// <summary>
        /// Написать комментарий
        /// </summary>
        /// <param name="commentDto">commentDto</param>
        public async Task<ActionResult<Request>> CreateComment(RequestCreateComment commentDto)
        {
            var request = await Repository.Get(x => (x.Id == commentDto.RequestId)).FirstOrDefaultAsync();
            request.Comment = commentDto.Comment;
            await Repository.UpdateAsync(request);
            return request;
        }

        /// <summary>
        /// Удалить заявку.
        /// </summary>
        /// <param name="id">id.</param>
        public async Task<ActionResult<Request>> Delete(long id)
        {
            var request = await Repository.Get().FirstOrDefaultAsync(x => x.Id == id);
            await DeleteAsync(id);
            return request;
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="requestDto">request.</param>
        public async Task<ActionResult<Request>> Create(RequestCreateDto requestDto)
        {
            var request = Mapper.Map<Request>(requestDto);
            request.Created = DateTime.Now;
            request.RequestStatus = RequestStatus.Waiting;
            await Repository.AddAsync(request);
            return request;
        }

        /// <summary>
        /// ArchiveExcel
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult<Request>> ArchiveExcel()
        {
            Excel.Application ex = new Excel.Application
            {
                Visible = true,
                SheetsInNewWorkbook = 1
            };
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);

            var requests = await Repository
                .Get()
                .Where(r => r.RequestStatus == RequestStatus.Done)
                .ProjectTo<RequestDto>(Mapper.ConfigurationProvider)
                .ToListAsync();

            sheet.Cells[1, 1] = "Заявка";
            sheet.Cells[1, 2] = "Описание";
            sheet.Cells[1, 3] = "Телефон";
            sheet.Cells[1, 4] = "Комментарий";
            sheet.Cells[1, 5] = "Организация";
            sheet.Cells[1, 6] = "Дата создания";
            sheet.Cells[1, 7] = "Дата завершения";

            for (int i = 2; i <= requests.Count + 1; i++)
            {
                sheet.Cells[i, 1] = requests[i - 2].Name;
                sheet.Cells[i, 2] = requests[i - 2].Description;
                sheet.Cells[i, 3] = requests[i - 2].PhoneNumber;
                sheet.Cells[i, 4] = requests[i - 2].Comment;
                sheet.Cells[i, 5] = requests[i - 2].Owner;
                sheet.Cells[i, 6] = requests[i - 2].Created;
                sheet.Cells[i, 7] = requests[i - 2].Completed;
            }

            Excel.Range range = sheet.get_Range("f2", "g" + requests.Count + 1);
            range.NumberFormat = "hh: mm: ss DD/MM/YYYY";
            sheet.Columns.AutoFit();
            sheet.Rows.AutoFit();

            workBook.Close();
            ex.Quit();

            return null;
        }
    }
}
