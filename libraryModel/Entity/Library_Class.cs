using System.ComponentModel.DataAnnotations;

namespace libraryModel.Entity
{
    public class Library_Class
    {
        [Key]
        public int Id { get; set; }
        public int bookId { get; set; }
        public string? book_location { get; set; }
        public string? book_quantity { get; set; }
        public DateTime? book_inserteddate { get; set; }
        public DateTime? book_updateddate { get; set; }
    }
}
