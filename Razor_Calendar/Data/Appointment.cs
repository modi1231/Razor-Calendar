

using System;

namespace Razor_Calendar.Data
{
    public class Appointment
    {
        //Parred down appointment class.  Just bare information needed to get done.  
        public Guid UID { get; set; }
        public DateTime START { get; set; }
        public DateTime STOP { get; set; }
        public DateTime DATE_ENTERED { get; set; }
    }
}
