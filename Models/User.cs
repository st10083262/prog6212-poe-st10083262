using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Study_Tracker.Models
{
    /* The User class hold the username and hashed password for the current user.
     * The class is static as we will only be needing one throughout the runtime of the program.
     */
    public class User
    {
        [Key, MaxLength(100)]
        public string username { get; set; } = null!; // The logged in Username.
        [MaxLength(64)]
        public string password { get; set; } = null!; // The password.
        public virtual ICollection<Module>? modules { get; set; } // The modules
    }
}
