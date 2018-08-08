﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.MyFinance;
using PTZ.HomeManagement.MyFinance.Models;

namespace PTZ.HomeManagement.MyFinance
{
    public interface IMyFinanceService
    {
        BankAccount GetBankAccountDefault(string userId);
        BankAccountMovement GetBankAccountMovementDefault(string userId, int bankAccountId);

        BankAccount GetBankAccount(string userId, int bankAccountId);
        List<BankAccount> GetBankAccounts(string userId);
        void SaveBankAccount(string userId, BankAccount bankAccount);
        void DeleteBankAccount(string userId, BankAccount bankAccount);

        BankAccountMovement GetBankAccountMovement(string userId, int bankAccountId, int movementId);
        List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, DateTime startDate, DateTime endDate);
        List<BankAccountMovement> GetBankAccountMovements(string userId, int bankAccountId, int qtdOfMovements = 1000, SortOrder dateSortOrder = SortOrder.Unspecified);
        void SaveBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement);
        List<BankAccountMovement> SaveBankAccountMovements(string userId, int bankAccountId, List<BankAccountMovement> list);
        void DeleteBankAccountMovement(string userId, int bankAccountId, BankAccountMovement bankAccountMovement);

        List<BankAccountMovement> ImportBankAccountMovement(string userId, int bankAccountId, BankAccountMovementImportType importType, IFormFile file);
    }
}