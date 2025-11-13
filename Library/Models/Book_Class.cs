using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book_Class
    {
        [Key]
        public int book_Id { get; set; }
        public string? book_name { get; set; }
        public string? book_author { get; set; }
        public string? book_publisher { get; set; }
        public string? book_publishYear { get; set; }
        public string? book_description { get; set; }
        public string? book_type { get; set; }
        public string? book_price { get; set; }   
        public string? book_language { get; set; }
        public string? book_pages { get; set; }
        public string? book_title { get; set; }
        public string? book_category { get; set; }
        public DateTime? inserteddate { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
