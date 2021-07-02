namespace Volunteers.Services.Services
{
    using System;
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

            DeletePreviousFile(filePath);

            // создание книги ЭКСЕЛЬ
            var workbook = new HSSFWorkbook();

            // добавление листа
            var sheet = workbook.CreateSheet("Заявки");

            CreateFirstRow(sheet);

            for (var i = 1; i < requests.Count + 1; i++)
            {
                // создание новой строки
                var row = sheet.CreateRow(i);

                var cell0 = row.CreateCell(0);
                cell0.SetCellValue(requests[i - 1].Name);

                var cell1 = row.CreateCell(1);
                cell1.SetCellValue(requests[i - 1].Description);

                var cell2 = row.CreateCell(2);
                cell2.SetCellValue(requests[i - 1].PhoneNumber);

                var cell3 = row.CreateCell(3);
                cell3.SetCellValue(requests[i - 1].Comment);

                var cell4 = row.CreateCell(4);
                cell4.SetCellValue(requests[i - 1].Owner);

                var cell5 = row.CreateCell(5);
                cell5.SetCellValue(Convert.ToDateTime(requests[i - 1].Created).ToString("yy-MM-dd HH:mm:ss"));

                var cell6 = row.CreateCell(6);
                cell6.SetCellValue(Convert.ToDateTime(requests[i - 1].Completed).ToString("yy-MM-dd HH:mm:ss"));
            }

            sheet.AutoSizeColumn(0);
            sheet.AutoSizeColumn(1);
            sheet.AutoSizeColumn(2);
            sheet.AutoSizeColumn(3);
            sheet.AutoSizeColumn(4);
            sheet.AutoSizeColumn(5);

            using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            workbook.Write(stream);
        }

        /// <summary>
        /// Создание первой строки для таблицы
        /// </summary>
        /// <param name="sheet">sheet</param>
        public void CreateFirstRow(NPOI.SS.UserModel.ISheet sheet)
        {
            var row = sheet.CreateRow(0);
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

        /// <summary>
        /// DeletePreviousFile
        /// </summary>
        /// <param name="filePath">files path</param>
        public void DeletePreviousFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
