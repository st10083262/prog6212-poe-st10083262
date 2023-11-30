using System.ComponentModel.DataAnnotations;

namespace Study_Tracker.Models
{
    public class StudyDate
    {
        [Key]
        public int SessionID { get; set; }
        public DateTime date { get; set; } 
        public double StudiedHours { get; set; } // How many hours were dedicated to studying today?

        public virtual Module module { get; set; } = null!;
    }
}
