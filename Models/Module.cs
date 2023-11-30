﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Study_Tracker.Models
{
    public class Module
    {

        [Key]
        [MaxLength(50)]
        public string moduleCode { get; set; } = null!; 
        [MaxLength(50)]
        public string moduleName { get; set; } = null!; 
        public int credits { get; set; } 
        public double classHoursPerWeek { get; set; } 
        [NotMapped]
        private double? hoursStudiedThisWeek; 
        public int semesterNumOfWeeks { get; set; } 
        public DateTime semesterStartDate { get; set; } 

        public virtual User user { get; set; } = null!;
        public virtual ICollection<StudyDate>? studyDates { get; set; } = null!;

        [NotMapped]
        public double HoursStudiedThisWeek
        {
            get
            {
                if(studyDates != null && studyDates.Count > 0)
                {
                    double hours = 0;
                    foreach(StudyDate entry in studyDates)
                    {
                        if (DatesAreInTheSameWeek(entry.date, DateTime.Now)) // If the date was this week.
                        {
                            hours += entry.StudiedHours; 
                        }
                    }
                    return hours;
                }
                return 0;
            }

            set
            {
                HoursStudiedThisWeek = value;
            }
        }

        /* Checks if 2 dates are in the same week.
         * The week starts on a Monday.
         * Taken from https://stackoverflow.com/questions/25795254/check-if-a-datetime-is-in-same-week-as-other-datetime
         * Authored by Sparrow on Aug 5, 2016 at 14:55
         */
        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)(cal.GetDayOfWeek(date1) - 1));
            var d2 = date2.Date.AddDays(-1 * (int)(cal.GetDayOfWeek(date2) - 1));

            return d1 == d2;
        }

        [NotMapped]
        public double TargetStudyHours
        {
            get
            {
                return ((credits * 10) / semesterNumOfWeeks) - classHoursPerWeek; 
            }

            set
            {
                TargetStudyHours = value;
            }
        }
    }
}
