﻿using AutoMapper;
using PTZ.HomeManagement.Models.ManageViewModels;
using PTZ.HomeManagement.Models.MyFinance;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, IndexViewModel>()
                .ForMember(vm => vm.IsEmailConfirmed, opt => opt.MapFrom(u => u.EmailConfirmed));

            CreateMap<List<BankAccount>, AccountListViewModel>()
                .ForMember(vm => vm.Items, opt => opt.MapFrom(u => Mapper.Map<IList<BankAccount>, IList<AccountListItemViewModel>>(u)));

            CreateMap<BankAccount, AccountListItemViewModel>();

            CreateMap<BankAccount, AccountViewModel>();
            CreateMap<AccountViewModel, BankAccount>();

            CreateMap<BankAccount, AccountMovementListViewModel>();
            CreateMap<BankAccountMovement, AccountMovementListItemViewModel>();

            CreateMap<BankAccountMovement, AccountMovementViewModel>();
            CreateMap<AccountMovementViewModel, BankAccountMovement>();

            CreateMap<BankAccount, DashboardAccountViewModel>()
               .ForMember(vm => vm.Amount, opt => opt.MapFrom(u => u.CurrentBalance))
               .ForMember(vm => vm.AssetType, opt => opt.MapFrom(u => u.AccountType))
               .ForMember(vm => vm.AccountNumber, opt => opt.MapFrom(u => u.IBAN))
               .ForMember(vm => vm.AccountTitle, opt => opt.MapFrom(u => u.Name));
        }
    }
}