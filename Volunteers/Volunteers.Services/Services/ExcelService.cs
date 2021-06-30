namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Volunteers.DB;
    using Volunteers.Entities;
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
        /// Экспорт
        /// </summary>
        /// <param name="requests">Список заявок.</param>
        public async Task<Stream> Export(List<RequestDto> requests)
        {
            var ex = new Excel.Application
            {
                Visible = true,
                SheetsInNewWorkbook = 1
            };
            var workBook = ex.Workbooks.Add(Type.Missing);
            var sheet = (Excel.Worksheet)ex.Worksheets.Item[1];

            sheet.Cells[1, 1] = "Заявка";
            sheet.Cells[1, 2] = "Описание";
            sheet.Cells[1, 3] = "Телефон";
            sheet.Cells[1, 4] = "Комментарий";
            sheet.Cells[1, 5] = "Организация";
            sheet.Cells[1, 6] = "Дата создания";
            sheet.Cells[1, 7] = "Дата завершения";

            for (var i = 2; i <= requests.Count + 1; i++)
            {
                sheet.Cells[i, 1] = requests[i - 2].Name;
                sheet.Cells[i, 2] = requests[i - 2].Description;
                sheet.Cells[i, 3] = requests[i - 2].PhoneNumber;
                sheet.Cells[i, 4] = requests[i - 2].Comment;
                sheet.Cells[i, 5] = requests[i - 2].Owner;
                sheet.Cells[i, 6] = requests[i - 2].Created;
                sheet.Cells[i, 7] = requests[i - 2].Completed;
            }

            var range = sheet.Range["f2", "g" + requests.Count + 1];
            range.NumberFormat = "hh: mm: ss DD/MM/YYYY";
            sheet.Columns.AutoFit();
            sheet.Rows.AutoFit();

            await using var stream = new MemoryStream();
            workBook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
