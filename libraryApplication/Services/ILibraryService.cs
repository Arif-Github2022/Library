using libraryApplication.Model;
using libraryModel.Entity;
namespace libraryApplication.Services
{
    public interface ILibraryService
    {
        Library_User_Model? ValidateCredentials(string userName, string password);
        IEnumerable<Library_User_Model> Search(string? q);
    }
}