using libraryApplication.Model;
using libraryModel;
using libraryModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace libraryApplication.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly List<Library_User_Model> _users = new()
    {
        new Library_User_Model{ username = "alice", password = "password1"},
        new Library_User_Model{ username = "bob", password = "password2"},
        new Library_User_Model{ username = "carol", password = "password3"},
        new Library_User_Model{ username = "mohammad", password = "P@ssw0rd"}
    };

        public Library_User_Model? ValidateCredentials(string? userName, string password)
        {
            return _users.FirstOrDefault(u =>
                string.Equals(u.username, userName, StringComparison.OrdinalIgnoreCase)
                && u.password == password);
        }

        public IEnumerable<Library_User_Model> Search(string? q)
        {
            if (string.IsNullOrWhiteSpace(q)) return _users;
            q = q.Trim().ToLowerInvariant();
            return _users.Where(u =>
                u.username.ToLowerInvariant().Contains(q));
        }
    }
}
