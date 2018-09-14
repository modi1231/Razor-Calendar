using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor_Calendar.Data
{
    public class DataAccess
    {

        public async Task<Appointment> GetAppointmentAsync()
        {
            Appointment temp = new Appointment()
            {
                UID = Guid.NewGuid(),
                DATE_ENTERED = DateTime.Now,
                START = new DateTime(2018, 09, 5),
                STOP = new DateTime(2018, 09, 14)
            };

            return temp;
        }
        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            List<Appointment> myReturn = new List<Appointment>();

            Appointment temp = new Appointment()
            {
                UID = Guid.NewGuid(),
                DATE_ENTERED = DateTime.Now,
                START = new DateTime(2018, 09, 5),
                STOP = new DateTime(2018, 09, 14)
            };

            myReturn.Add(temp);

            temp = new Appointment()
            {
                UID = Guid.NewGuid(),
                DATE_ENTERED = DateTime.Now,
                START = new DateTime(2018, 09, 27),
                STOP = new DateTime(2018, 09, 27)
            };

            myReturn.Add(temp);

            temp = new Appointment()
            {
                UID = Guid.NewGuid(),
                DATE_ENTERED = DateTime.Now,
                START = new DateTime(2018, 09, 30),
                STOP = new DateTime(2018, 10, 1)
            };

            myReturn.Add(temp);

            return myReturn;
        }
    }
}
