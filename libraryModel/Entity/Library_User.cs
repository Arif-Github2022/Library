using System.ComponentModel.DataAnnotations;

namespace libraryModel.Entity
{
    public class Library_User
    {
        [Key]
        public int userId  { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
    }
}
