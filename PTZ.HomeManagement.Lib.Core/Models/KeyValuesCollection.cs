using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PTZ.HomeManagement.Models
{
    public class KeyValuesCollection
    {
        public int Id { get; set; }
        public Guid AppId { get; set; }
        public Guid InstanceId { get; set; }

        [MaxLength(2000)]
        public string Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
