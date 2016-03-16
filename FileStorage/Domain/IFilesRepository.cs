using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FileStorage.Domain
{
    public interface IFilesRepository
    {
        IList<File> GetFiles(string userId);
        Task<IList<File>> GetFilesAsync(string userId);
        void AddFile(File file, byte[] fileBytes);
        void EditFile(File file);
        void DeleteFile(int id);
        File GetFile(int id);
        void AddFileAsync(File file, byte[] fileBytes);
        void EditFileAsync(File file);
        void DeleteFileAsync(int id);
        Task<File> GetFileAsync(int id);

        IList<Category> GetCategories(string userId);
        Task<IList<Category>> GetCategoriesAsync(string userId);
        void AddCategory(Category category);
        void EditCategory(Category cat);
        void DeleteCategory(int id);
        Category GetCategory(int id);
        void AddCategoryAsync(Category cat);
        void EditCategoryAsync(Category cat);
        void DeleteCategoryAsync(int id);
        Task<Category> GetCategoryAsync(int id);

        byte[] GetPhysicalFile(int id);
        Task<byte[]> GetPhysicalFileAsync(int id);

        IList<ApplicationUser> GetUsers();
        ApplicationUser GetUserByUserName(string username);
        void EditUser(ApplicationUser user);
        void EditUserAsync(ApplicationUser user);
    }
}