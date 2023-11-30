using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Study_Tracker.Models
{
    
    public class User
    {
        [Key, MaxLength(100)]
        public string username { get; set; } = null!; //username.
        [MaxLength(64)]
        public string password { get; set; } = null!; //password.
        public virtual ICollection<Module>? modules { get; set; } 
    }
}
