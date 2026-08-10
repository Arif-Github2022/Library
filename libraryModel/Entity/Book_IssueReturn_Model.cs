using System.ComponentModel.DataAnnotations;

namespace libraryApplication.Model
{
    public class Book_IssueReturn_Model
    {
        public int Id { get; set; }
        public int book_Id { get; set; }
        public int userId { get; set; }
        public DateTime? isuuedate { get; set; }
        public DateTime? returndate { get; set; }
        public string? issuedby { get; set; }
        public string? receivedby { get; set; }
        public DateTime? inserteddate { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
