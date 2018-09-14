using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor_Calendar.Data
{
    public class MyDay
    {
        //Bare information for a 'day' in the calendar.  
        public DateTime aptDay { get; set; }
        public bool isOccupied { get; set; }

        public string GetNumber()
        {
            string ret = string.Empty;

            if (aptDay == null || aptDay == DateTime.MinValue)
                ret = string.Empty;
            else
                ret = aptDay.Day.ToString();

            return ret;
        }
    }
}
