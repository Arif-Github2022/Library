using Microsoft.EntityFrameworkCore;
using libraryModel.Entity;

namespace libraryInfra
{
    public class Library_Context : DbContext
    {
        public Library_Context(DbContextOptions<Library_Context> options) : base(options) { }
        public DbSet<Member_Class> Member_Class { get; set; }
        public DbSet<Book_Class> Book_Class { get; set; }
        public DbSet<Book_IssueReturn_Class> book_IssueReturn_Class { get; set; }
        public DbSet<Library_User> Library_User { get; set; }
        public DbSet<Library_Class> library_Class { get; set; }

    }
}

