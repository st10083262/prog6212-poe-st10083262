using System.ComponentModel.DataAnnotations;

namespace Study_Tracker.Models
{
    public class StudyDate
    {
        [Key]
        public int studyDateID { get; set; }
        public DateTime date { get; set; } // Set the date of studying to today.
        public double hoursStudied { get; set; } // How many hours were studied today.

        public virtual Module module { get; set; } = null!;
    }
}
