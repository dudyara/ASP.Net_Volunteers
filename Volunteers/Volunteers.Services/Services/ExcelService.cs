namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using Volunteers.Services.Dto;

    /// <summary>
    /// ExcelService
    /// </summary>
    public class ExcelService
    {
        /// <summary>
        /// Export
        /// </summary>
        /// <param name="requests">requests</param>
        /// <param name="stream">stream</param>
        public void Export(List<RequestDto> requests, out Stream stream)
        {
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

                var isCreatedValid = DateTime.TryParse(requests[i - 1].Created, out var created);
                row.CreateCell(5).SetCellValue(isCreatedValid ? $"{created:yy-MM-dd HH:mm}" : "Время не указано!");

                if (Convert.ToDateTime(requests[i - 1].Completed) == DateTime.MinValue)
                {
                    row.CreateCell(6).SetCellValue("Не завершен");
                }
                else
                {
                    row.CreateCell(6).SetCellValue(isCreatedValid ? $"{created:yy-MM-dd HH:mm}" : "Время не указано");
                }
            }

            sheet.AutoSizeColumn(0);
            sheet.AutoSizeColumn(1);
            sheet.AutoSizeColumn(2);
            sheet.AutoSizeColumn(3);
            sheet.AutoSizeColumn(4);
            sheet.AutoSizeColumn(5);

            stream = new MemoryStream();
            workbook.Write(stream);
            stream.Position = 0;
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
    }
}
