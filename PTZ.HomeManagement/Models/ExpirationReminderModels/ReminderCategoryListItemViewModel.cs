using System.ComponentModel;

namespace PTZ.HomeManagement.Models.ExpirationReminderModels
{
    public class ReminderCategoryListItemViewModel
    {
        [DisplayName("Id")]
        public long Id { get; set; }
        
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}