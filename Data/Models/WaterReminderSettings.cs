using System;
using System.Collections.Generic;
using System.Text;

namespace omni_multitool.Data.Models
{
    public class WaterReminderSettings
    {
        
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int IntervalInMinutes { get; set; }
        public bool PlaySound { get; set; }
    }
}
