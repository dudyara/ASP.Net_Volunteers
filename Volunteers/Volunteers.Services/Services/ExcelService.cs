namespace Volunteers.Services.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
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
        /// ExportExcel
        /// </summary>
        /// <param name="requestDto">requestDto.</param>
        public async Task<Stream> ExportExcel(RequestFilterDto requestDto)
        {
            Excel.Application ex = new Excel.Application
            {
                Visible = true,
                SheetsInNewWorkbook = 1
            };
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);

            RequestService reqService = new RequestService(Mapper, Repository, Validator);
            var requests = await reqService.Get(requestDto);

            sheet.Cells[1, 1] = "Заявка";
            sheet.Cells[1, 2] = "Описание";
            sheet.Cells[1, 3] = "Телефон";
            sheet.Cells[1, 4] = "Комментарий";
            sheet.Cells[1, 5] = "Организация";
            sheet.Cells[1, 6] = "Дата создания";
            sheet.Cells[1, 7] = "Дата завершения"; 

            for (int i = 2; i <= requests.Count + 1; i++)
            {
                sheet.Cells[i, 1] = requests.Result[i - 2].Name;
                sheet.Cells[i, 2] = requests.Result[i - 2].Description;
                sheet.Cells[i, 3] = requests.Result[i - 2].PhoneNumber;
                sheet.Cells[i, 4] = requests.Result[i - 2].Comment;
                sheet.Cells[i, 5] = requests.Result[i - 2].Owner;
                sheet.Cells[i, 6] = requests.Result[i - 2].Created;
                sheet.Cells[i, 7] = requests.Result[i - 2].Completed;
            }

            Excel.Range range = sheet.get_Range("f2", "g" + requests.Count + 1);
            range.NumberFormat = "hh: mm: ss DD/MM/YYYY";
            sheet.Columns.AutoFit();
            sheet.Rows.AutoFit();
            using MemoryStream stream = new MemoryStream();
            workBook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
