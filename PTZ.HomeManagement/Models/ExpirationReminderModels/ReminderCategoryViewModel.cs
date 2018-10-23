using System.ComponentModel;

namespace PTZ.HomeManagement.Models.ExpirationReminderModels
{
    public class ReminderCategoryViewModel
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
    }
}