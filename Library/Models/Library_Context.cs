using eTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
    public class Library_Context : DbContext
    {
        public DbSet<Member_Class> member_Class { get; set; }
        public DbSet<Book_Class> book_Class { get; set; }
        public DbSet<Library_Class> library_Class { get; set; }
        public DbSet<Library_User> user_Class { get; set; }
        public DbSet<Book_IssueReturn_Class> book_IssueReturn_Class { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=library;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        public Library_Context()
        {
        }

        public Library_Context(DbContextOptions<Library_Context> options)
       : base(options)
        {
        }      
       
    }
}
