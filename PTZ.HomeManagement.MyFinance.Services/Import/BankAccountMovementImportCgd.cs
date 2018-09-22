using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance.Imports
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
                        decimal amount = !string.IsNullOrEmpty(words[4].Replace(".", "")) ? -decimal.Parse(words[4].Replace(".", "")) : decimal.Parse(words[5].Replace(".", ""));

                        list.Add(new BankAccountMovement()
                        {
                            MovementDate = DateTime.Parse(words[0]),
                            ValueDate = DateTime.Parse(words[1]),
                            Description = words[2],
                            Amount = amount,
                            TotalBalanceAfterMovement = decimal.Parse(words[6].Replace(".", "")),
                        });
                    }
                }
            }

            return list;
        }
    }
}
