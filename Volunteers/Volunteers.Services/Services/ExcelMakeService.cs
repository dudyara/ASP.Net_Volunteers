
namespace Volunteers.Services.Services
{
    using System.Collections.Generic;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using Volunteers.Services.Dto;

    /// <summary>
    /// ExcelMakeService
    /// </summary>
    public class ExcelMakeService
    {
        /// <summary>
        /// Export
        /// </summary>
        /// <param name="requests">requests</param>
        /// <param name="filePath">filePath</param>
        public void Export(List<RequestDto> requests, string filePath)
        {
            // создание книги ЭКСЕЛЬ
            var workbook = new HSSFWorkbook();

            // добавление листа
            var sheet = workbook.CreateSheet("Data");

            for (var i = 0; i < requests.Count; i++)
            {
                // создание новой строки
                var row = sheet.CreateRow(i);

                // если это заголовок 
                if (i == 0)
                {
                    var xp = 5;
                    var tp = 10;
                     
                    var cell0 = row.CreateCell(0);
                    cell0.SetCellValue("Заявка");
                    var cell1 = row.CreateCell(1);
                    cell1.SetCellValue("Описание");
                    var cell2 = row.CreateCell(2);
                    cell2.SetCellValue("Телефон");
                    var cell3 = row.CreateCell(3);
                    cell3.SetCellValue("Комментарий");
                    var cell4 = row.CreateCell(4);
                    cell4.SetCellValue("Организация");
                    var cell5 = row.CreateCell(5);
                    cell5.SetCellValue("Дата создания");
                    var cell6 = row.CreateCell(6);
                    cell6.SetCellValue("Дата завершения");
                }

                // иначе
                else
                {
                    var cell0 = row.CreateCell(0);
                    cell0.SetCellValue(requests[i].Name);

                    var cell1 = row.CreateCell(1);
                    cell1.SetCellValue(requests[i].Description);

                    var cell2 = row.CreateCell(2);
                    cell2.SetCellValue(requests[i].PhoneNumber);

                    var cell3 = row.CreateCell(3);
                    cell3.SetCellValue(requests[i].Comment);

                    var cell4 = row.CreateCell(4);
                    cell4.SetCellValue(requests[i].Owner);

                    var cell5 = row.CreateCell(5);
                    cell5.SetCellValue(requests[i].Created);

                    var cell6 = row.CreateCell(6);
                    cell6.SetCellValue(requests[i].Completed); 
                }
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }
    }
}
