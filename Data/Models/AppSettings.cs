using System;
using System.Collections.Generic;
using System.Text;

namespace omni_multitool.Data.Models
{
    public class AppSettings
    {
        public int Id { get; set; }
        public bool StartWithWindows { get; set; }
        public bool MinimizeToTray { get; set; }
        public bool ShowWelcomePage { get; set; }
    }
}
