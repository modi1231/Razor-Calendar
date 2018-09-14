using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Calendar.Data;

/*
 * Goals:
 * -x- display custom calendar
 * -x- next month/previous month
 * -- admin to edit/add
 * -x- color code display
 * -- show amalgmated of things in the queue
 * -x- InitGrid should have 'incoming' collection.. to match up dates blocked... single appointment or list of appointments.
 * could scap it all andoperate on a 2d array list of type 'date'.. fill it in by code.. and have the display just.. well.. display.
 */
namespace Razor_Calendar.Pages
{
    public class IndexModel : PageModel
    {
       public string currentMonth { get; set; }//Help determine what we are looking at.

        public List<string> dayNames { get; set; }//Could be an enum.

        public DateTime current { get; set; }

        public MyDay[,] dayCollection { get; set; }


        public void OnGet(DateTime tempDate)
        {
            // setup dispaly
            dayNames = new List<string>();
            dayNames.Add("Sun");
            dayNames.Add("Mon");
            dayNames.Add("Tues");
            dayNames.Add("Wed");
            dayNames.Add("Thur");
            dayNames.Add("Fri");
            dayNames.Add("Sat");

            // fresh to the page or from a previous page.
            if (tempDate == null || tempDate == DateTime.MinValue)
                current = DateTime.Now;// new DateTime(2018, 12, 05);
            else
                current = tempDate;

            //Setting all the particulars.
            currentMonth = current.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);
            int startingDay = (int)new DateTime(current.Year, current.Month, 1).DayOfWeek;// so it lines up right.
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month); // helps stop creating the grid.

            //testing out blocking out things.
            DataAccess _data = new DataAccess();
            //Appointment appt = _data.GetAppointmentAsync().Result;
            List<Appointment> allAppts = _data.GetAppointmentsAsync().Result;


            InitGrid(current.Year, current.Month, daysInMonth, startingDay);

            //SetAppointment(current.Year, current.Month, appt);
            SetAppointments(current.Year, current.Month, allAppts);
        }

        //Next/previous month are clicked.  This changes the 'current' and must be redrawn.
        public IActionResult OnPostNextMonth(int direction, DateTime curr)
        {
            if (direction == 0)
            {
                curr = curr.AddMonths(-1);
            }
            else
            {
                curr = curr.AddMonths(1);
            }

            return RedirectToAction(" / Index", new { tempDate = curr });
        }

        //Set up the grid
        private void InitGrid(int year, int month, int daysInMonth, int startingDay)
        {
            int lCurrentDay = 0; // Makes sure the arrays do not have more than expected number of days dealt with
            bool end = false;

            dayCollection = new MyDay[6, 7];

            for (int i = 0; i < 6 && !end; i++)
            {
                for (int z = 0; z < 7; z++)
                {
                    if (lCurrentDay > 0 || z == startingDay)
                    {
                        lCurrentDay += 1;
                        if (lCurrentDay > daysInMonth)
                        {
                            end = true;
                            break;
                        }
                        dayCollection[i, z] = new MyDay()
                        {
                            aptDay = new DateTime(year, month, lCurrentDay),
                            isOccupied = false
                        };
                    }
                }
            }
        }

        //Comapre the array days with the appointment days.
        private void SetAppointment(int year, int month, Appointment temp)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int z = 0; z < 7; z++)
                {
                    if (dayCollection[i, z] != null && (dayCollection[i, z].aptDay >= temp.START && dayCollection[i, z].aptDay <= temp.STOP))
                    {
                        dayCollection[i, z].isOccupied = true;
                    }
                }
            }
        }
        private void SetAppointments(int year, int month, List<Appointment> temp)
        {
            foreach (Appointment item in temp)
            {
                SetAppointment(year, month, item);
            }
        }
    }
}