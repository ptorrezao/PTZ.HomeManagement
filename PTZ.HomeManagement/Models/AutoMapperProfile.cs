﻿using AutoMapper;
using PTZ.HomeManagement.ExpirationReminder.Core;
using PTZ.HomeManagement.Models.ExpirationReminderModels;
using PTZ.HomeManagement.Models.ManageViewModels;
using PTZ.HomeManagement.Models.MyFinanceViewModels;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.Utils;
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

            //MyFinance
            CreateMap<List<BankAccount>, AccountListViewModel>()
                .ForMember(vm => vm.Items, opt => opt.MapFrom(u => Mapper.Map<IList<BankAccount>, IList<AccountListItemViewModel>>(u)));

            CreateMap<BankAccount, AccountListItemViewModel>();

            CreateMap<BankAccount, AccountViewModel>();
            CreateMap<AccountViewModel, BankAccount>();

            CreateMap<BankAccount, AccountMovementListViewModel>();
            CreateMap<BankAccountMovement, AccountMovementListItemViewModel>();

            CreateMap<BankAccountMovement, AccountMovementViewModel>();
            CreateMap<AccountMovementViewModel, BankAccountMovement>();

            CreateMap<BankAccountMovement, CategoriesAccountMovementViewModel>()
                .ForMember(v => v.SelectedCategories, opt => opt.MapFrom(q => q.Categories.Select(x => x.CategoryId)));
            CreateMap<CategoriesAccountMovementViewModel, BankAccountMovement>();

            CreateMap<BankAccount, LineChartItemViewModel>()
               .ForMember(vm => vm.Amount, opt => opt.MapFrom(u => u.CurrentBalance))
               .ForMember(vm => vm.Color, opt => opt.MapFrom(u => u.Color))
               .ForMember(vm => vm.Group, opt => opt.MapFrom(u => u.Bank));

            CreateMap<BankAccount, DoughnutChartItemViewModel>()
               .ForMember(vm => vm.Amount, opt => opt.MapFrom(u => u.CurrentBalance))
               .ForMember(vm => vm.AssetType, opt => opt.MapFrom(u => u.AccountType.GetDescription()))
               .ForMember(vm => vm.AccountNumber, opt => opt.MapFrom(u => u.IBAN))
               .ForMember(vm => vm.Color, opt => opt.MapFrom(u => u.Color))
               .ForMember(vm => vm.Group, opt => opt.MapFrom(u => u.Bank))
               .ForMember(vm => vm.AccountTitle, opt => opt.MapFrom(u => u.Name));

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
            CreateMap<List<Category>, CategoryListViewModel>()
            .ForMember(vm => vm.Items, opt => opt.MapFrom(u => Mapper.Map<IList<Category>, IList<CategoryListItemViewModel>>(u)));

            //Expiration Reminder
            CreateMap<List<Reminder>, ReminderListViewModel>()
                .ForMember(vm => vm.Items, opt => opt.MapFrom(u => Mapper.Map<IList<Reminder>, IList<ReminderListItemViewModel>>(u)));

            CreateMap<Reminder, ReminderListItemViewModel>();

            CreateMap<Reminder, ReminderViewModel>();
            CreateMap<ReminderViewModel, Reminder>();
        }
    }
}
