using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance.Imports
{
    public class BankAccountMovementImportBpi : BankAccountMovementImport
    {
        public override List<BankAccountMovement> GetBankAccountMovements(IFormFile file)
        {
            List<BankAccountMovement> list = new List<BankAccountMovement>();

            if (file.OpenReadStream() == null || file.Length == 0)
                return list;

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int importStartsAtRow = 14;
                bool noMoreRows = false;
                int row;
                while (!noMoreRows)
                {
                    row = importStartsAtRow++;

                    object movDate = worksheet.Cells[row, 1].Value;
                    object valDate = worksheet.Cells[row, 2].Value;
                    object desc = worksheet.Cells[row, 3].Value;

                    if (movDate == null &&
                        valDate == null && 
                        desc == null)
                    {
                        // By convetion this means there is no more records
                        break;
                    }

                    decimal amount = decimal.Parse(worksheet.Cells[row, 4].Value.ToString().Replace(".", ""));
                    decimal total = decimal.Parse(worksheet.Cells[row, 5].Value.ToString().Replace(".", ""));

                    list.Add(new BankAccountMovement()
                    {
                        MovementDate = DateTime.Parse(movDate.ToString()),
                        ValueDate = DateTime.Parse(string.IsNullOrEmpty(valDate.ToString()) ? movDate.ToString() : valDate.ToString()),
                        Description = desc.ToString(),
                        Amount = amount,
                        TotalBalanceAfterMovement = total,
                    });
                }
            }


            return list;
        }
    }
}