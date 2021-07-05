namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        /// <param name="message">message</param>
        /// <param name="filePath">filePath</param>
        public void Export(string message, string filePath)
        {
            // создание книги ЭКСЕЛЬ
            var workbook = new HSSFWorkbook();

            // добавление листа
            var sheet = workbook.CreateSheet("Заявки");

            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(message);
        }

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

                row.CreateCell(0).SetCellValue(requests[i - 1].Name);

                row.CreateCell(1).SetCellValue(requests[i - 1].Description);

                row.CreateCell(2).SetCellValue(requests[i - 1].PhoneNumber);

                row.CreateCell(3).SetCellValue(requests[i - 1].Comment);

                row.CreateCell(4).SetCellValue(requests[i - 1].Owner);

                row.CreateCell(5).SetCellValue(Convert.ToDateTime(requests[i - 1].Created).ToString("yy-MM-dd HH:mm"));

                row.CreateCell(6).SetCellValue(Convert.ToDateTime(requests[i - 1].Completed).ToString("yy-MM-dd HH:mm"));
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
        private void CreateFirstRow(NPOI.SS.UserModel.ISheet sheet)
        {
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("Заявка");
            row.CreateCell(1).SetCellValue("Описание");
            row.CreateCell(2).SetCellValue("Телефон");
            row.CreateCell(3).SetCellValue("Комментарий");
            row.CreateCell(4).SetCellValue("Организация");
            row.CreateCell(5).SetCellValue("Дата создания");
            row.CreateCell(6).SetCellValue("Дата завершения");
        }

        /// <summary>
        /// DeletePreviousFile
        /// </summary>
        /// <param name="filePath">files path</param>
        private void DeletePreviousFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
