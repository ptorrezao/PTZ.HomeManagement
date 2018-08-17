using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class CategoriesAccountMovementViewModel : AccountMovementViewModel
    {
        [DisplayName("SelectedCategories")]
        public List<long> SelectedCategories { get; set; }

        public List<SelectListItem> AvailableCategories { get; set; }

        public void SetAvailableCategories(List<CategoryViewModel> value)
        {
            this.AvailableCategories = new List<SelectListItem>();
            this.SelectedCategories = this.SelectedCategories ?? new List<long>();
            foreach (var item in value)
            {
                AvailableCategories.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Name, Selected = this.SelectedCategories.Contains(item.Id) });
            }
        }
    }
}