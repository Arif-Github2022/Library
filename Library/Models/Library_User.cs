using System.ComponentModel.DataAnnotations;

namespace eTrack.Models
{
    public class Library_User
    {
        [Key]
        public int userId  { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
    }
}
