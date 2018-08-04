using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.Models.MyFinance;

namespace PTZ.HomeManagement.Services.MyFinance
{
    public class BankAccountMovementImportCgd : BankAccountMovementImport
    {
        public override List<BankAccountMovement> GetBankAccountMovements(IFormFile file)
        {
            List<BankAccountMovement> list = new List<BankAccountMovement>();

            using (TextReader reader = new StreamReader(file.OpenReadStream()))
            {
                for (var i = 0; i < 7; i++)
                {
                    reader.ReadLine();
                }

                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(';');
                    if (!string.IsNullOrEmpty(words[0].Replace(" ", "")))
                    {
                        decimal amount = !string.IsNullOrEmpty(words[3]) ? -decimal.Parse(words[3]) : decimal.Parse(words[4]);

                        list.Add(new BankAccountMovement()
                        {
                            MovementDate = DateTime.Parse(words[0]),
                            ValueDate = DateTime.Parse(words[1]),
                            Description = words[2],
                            Amount = amount,
                            TotalBalanceAfterMovement = decimal.Parse(words[5]),
                        });
                    }
                }
            }

            return list;
        }
    }
}
