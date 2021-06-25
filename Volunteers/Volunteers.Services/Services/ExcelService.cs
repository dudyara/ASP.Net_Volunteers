namespace Volunteers.Services.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;
    using Excel = Microsoft.Office.Interop.Excel;

    /// <summary>
    /// ExcelService
    /// </summary>
    public class ExcelService : BaseService<Request, RequestDto>
    {
        /// <summary>
        /// Метод заявок.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="repository">repository</param>
        /// <param name="validator">validator</param>
        public ExcelService(IVolunteerMapper mapper, IDbRepository<Request> repository, IDtoValidator validator)
            : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// ArchiveExcel
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> ArchiveExcel()
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

            var memoryStream = new MemoryStream();
            workBook.SaveAs(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
