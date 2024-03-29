# Razor-Calendar
ASP.NET C# Core 2.1 
A bare bones custom calendar functionality tutorial for DIC.

https://www.dreamincode.net/forums/topic/412700-aspnet-razor-pages-core-21-custom-calendar/



=================
dreamincode.net tutorial backup ahead of decommissioning


 Post icon  Posted 14 September 2018 - 06:57 PM 


[b][u]Requirements:[/b][/u]
Visual Studio 2017
Core 2.1 / Razor pages
C#

[b][u]Github:[/b][/u]
https://github.com/modi1231/Razor-Calendar

[img]https://i.imgur.com/1llzONx.png[/img]

[b][u]Background:[/b][/u]
I had a need to add in a less-than-fancy calendar to a website.  To normal folk this would just show a generic days of being blocked out, but to admin who are logged in they can see what appointments were on each day.  At the time existing solutions were turned down so I had forged ahead to make a lightweight add-on to display appointments and play nice with the site's CSS.  The project had to also allow for flexibility if the client wanted to expanded what the calendar shows (holidays, messages, different colors, etc).  The final requirement was to have the ability to go forward/backwards by increment of one month.  

Here is the boiled down - to the bare necessities (Look for the bare necessities; The simple bare necessities) - on face complex task to the needed elements that highlight fun Razor functionality.

[b][u]Outline:[/b][/u]
Planning was straight forward.  I needed a representation of a calendar 'day', a collection of these 'day' objects, and appointment information.  For the sake of getting it down I ended up with a 6x7 array of object 'day'.  It was the quickest design choice to deal with when displaying the data in the html.

[b][u]Tutorial:[/b][/u]
I start with a new, empty, 2.1, web project in Visual Studios, add the appropriate bits (startup info, layout, wwwroot folders, etc).

Once the site scaffolding is created, I turn to my data object.

At its heart - the Appointment object is basic.  An id, start time, end time, and when it was entered.  This could be expanded out to include user IDs, staff IDs, names, custom color coding, etc.

[code]    public class Appointment
    {
        //Parred down appointment class.  Just bare information needed to get done.  
        public Guid UID { get; set; }
        public DateTime START { get; set; }
        public DateTime STOP { get; set; }
        public DateTime DATE_ENTERED { get; set; }
    }[/code]


MyDay object is what will be used for the HTML rendering.  In this tutorial all I need to know is the day it represents and if it is occupied. 

[code]    public class MyDay
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
    }[/code]


I like to keep my data abstracted (as one should) in a singe 'DataAccess' class.  This is really handy to setup early because, for testing, I can just have it spit out what ever and go on making the app versus having to the who database part here.

Right now I am just returning instances of the appointment object or a list of appointments.  (The latter being what I'll use after this is over).

[code]    public class DataAccess
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
    }[/code]


The Index model is equally simple.  Things we need to display.. the month's name.. (though I could juke around that, but it was just easier to have it done inside the server side code), the day names (display as well), the collection of 'days' for the month, and the current day.  That last one will be part of the backward/forward buttons.

[code]       public string currentMonth { get; set; }//Help determine what we are looking at.

        public List<string> dayNames { get; set; }//Could be an enum.

        public DateTime current { get; set; }

        public MyDay[,] dayCollection { get; set; }[/code]


For the most part this will have one action - the OnGet.  It will handle hitting the page for the first time, and any next/previous calls.  

Chiefly - every page load, OnGet's job is to setup everything to be displayed.  The day of the week names, current month name, etc.  Fairly rote stuff.

The nifty part is when you are using the initializing the grid and the setting of appointment.  

For the initialization I am doing an actual one to one representation of a calendar month as an array.  No fancy data collections.. just a plain old array.  This brought on some tricky parts.  Where in the first row do you start the calendar?  What happens when your current month has no more days?  Thankfully the Datetime class has the bulk of these figured out, and a quick application of a few for loops and the array is set!

[code]        private void InitGrid(int year, int month, int daysInMonth, int startingDay)
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
        }[/code]


Setting the appointment was another exercise of inner-loop/loop cooperation.  The only trick is to cycle through the array and compare if the 'current' day falls between the alert start and end.

One nifty feature is how this simplistically handles appointments that are part in one month and carry over to next month.  No new code needed for that!


   [code]     private void SetAppointment(int year, int month, Appointment temp)
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
        }[/code]

The Index HTML is pretty straight forward layout.
Month Name
Back/Forward buttons
Grid of the calendar.  

Interesting work flow note - when getting the GUI settled in I have a habbit of pushing around what I want with the Razor code, and then take all of that into the code behind to enforce separation of business code from display code.  I kept the chunk in there to compare how complex that was getting as compared to a small for loop that just dealt with the array of day objects.


As with all the Razor pages (that you want to have interact with the code behind) There's a big form method post to put it all inside.  

A classic example of the page interacting with the model.
[code]    <h3>@Model.currentMonth - @Model.current.Year</h3>[/code]

The buttons highlight two great Razor feautres.. the 'Handler' which directs Razor to the function you want to call, and routing information which provides parameter input.  

[code]
<button type="submit" asp-page-handler="NextMonth" asp-route-direction="0" asp-route-curr="@Model.current" style=""><</button>
<button type="submit" asp-page-handler="NextMonth" asp-route-direction="1" asp-route-curr="@Model.current" style="">></button>[/code]


The box display is pretty generic, but a few twists in there to spice it up.

One is how I conditionally added CSS class to a div.  Using an in-place IF I was able to test for null objects and if they met a condition!

[code]
<div class="box boxborder @(Model.dayCollection[i, z] != null && Model.dayCollection[i, z].isOccupied ? " occupied" : "")">[/code]


The proper IF-ELSE utilizes the @ for Razor code... calls functions from objects (GetNumber), and even spits out conditional HTML.  Pretty quick and snazzy as it doesn't do much processing on it's end, but just reacts to the data present.

[code]     
@if (Model.dayCollection[i, z] != null)
{
   <span>@Model.dayCollection[i, z].GetNumber()</span>
   if (Model.dayCollection[i, z].isOccupied)
   {
      <br /><span>O</span>
   }
}
else
{
   <span>&nbsp;</span>
}
[/code]

That wraps up the ninja calendar app project!  Covering requirements to class needs to minimal functions and ultimately the display.  It is not the most in depth use of Razor, but the code is darn portable and stylizable.  


[b][u]Advanced options:[/b][/u]
Make DB driven where only 'current month' appointments are pulled.
Make an Admin page to enter/edit/remove appointments.
