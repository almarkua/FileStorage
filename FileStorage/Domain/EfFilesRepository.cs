using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FileStorage.Domain
{
    public class EfFilesRepository:IFilesRepository
    {
        private readonly FileStorageContext _context;
        public EfFilesRepository(FileStorageContext context)
        {
            _context = context;
        }

        public IList<File> GetFiles(string userId)
        {
            return (userId == null)
                ? _context.Files.ToList()
                : _context.Files.Where(f => f.User.Id == userId).ToList();
        }

        public async Task<IList<File>> GetFilesAsync(string userId)
        {
            return await Task<IList<File>>.Factory.StartNew(() => GetFiles(userId));
        }

        public void AddFile(File file, byte[] fileBytes)
        {
            if (file.User == null) throw new NullReferenceException("File.User can't be null.");
            _context.Files.Add(file);
            file.User.UsedSpace += file.Size;
            _context.SaveChanges();
            AddPhysicalFile(file, fileBytes);
        }

        public void EditFile(File file)
        {
            var oldFile = _context.Files.FirstOrDefault(f => f.FileId == file.FileId);
            if (oldFile == null) return;
            oldFile.ContentType = file.ContentType;
            oldFile.Description = file.Description;
            oldFile.IsShared = file.IsShared;
            oldFile.Name = file.Name;
            oldFile.Size = file.Size;
            _context.SaveChanges();
        }

        public void DeleteFile(int id)
        {
            var file = _context.Files.FirstOrDefault(f => f.FileId == id);
            if (file == null) return;
            file.User.UsedSpace -= file.Size;
            _context.Files.Remove(file);
            DeletePhysicalFile(file);
            _context.SaveChanges();
        }

        public File GetFile(int id)
        {
            return _context.Files.FirstOrDefault(f => f.FileId == id);
        }

        public async void AddFileAsync(File file, byte[] fileBytes)
        {
            if (file.User == null) throw new NullReferenceException("File.User can't be null.");
            _context.Files.Add(file);
            AddPhysicalFileAsync(file, fileBytes);
            await _context.SaveChangesAsync();
        }

        public async void EditFileAsync(File file)
        {
            var oldFile = _context.Files.FirstOrDefault(f => f.FileId == file.FileId);
            if (oldFile == null) return;
            oldFile.ContentType = file.ContentType;
            oldFile.Description = file.Description;
            oldFile.IsShared = file.IsShared;
            oldFile.Name = file.Name;
            oldFile.Size = file.Size;
            await _context.SaveChangesAsync();
        }

        public async void DeleteFileAsync(int id)
        {
            var file = _context.Files.FirstOrDefault(f => f.FileId == id);
            if (file == null) return;
            _context.Files.Remove(file);
            DeletePhysicalFileAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task<File> GetFileAsync(int id)
        {
            return await Task<File>.Factory.StartNew(() =>
            {
                return _context.Files.FirstOrDefault(f => f.FileId == id);
            });
        }

        public IList<Category> GetCategories(string userId)
        {
            return (userId == null)
                ? _context.Categories.ToList()
                : _context.Categories.Where(cat => cat.User.Id == userId).ToList();
        }

        public Task<IList<Category>> GetCategoriesAsync(string userId)
        {
            return Task<IList<Category>>.Factory.StartNew(() => GetCategories(userId));
        }

        public void AddCategory(Category category)
        {
            if (category.User == null) throw new NullReferenceException("Category.User can`t be null");
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void EditCategory(Category category)
        {
            var oldCategory = _context.Categories.FirstOrDefault(cat => cat.CategoryId == category.CategoryId);
            if (oldCategory == null) return;
            oldCategory.Description = category.Description;
            oldCategory.Name = category.Name;
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(cat => cat.CategoryId == id);
            if (category == null) return;
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(cat => cat.CategoryId == id);
        }

        public async void AddCategoryAsync(Category category)
        {
            if (category.User == null) throw new NullReferenceException("Category.User can`t be null");
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async void EditCategoryAsync(Category category)
        {
            var oldCategory = _context.Categories.FirstOrDefault(cat => cat.CategoryId == category.CategoryId);
            if (oldCategory == null) return;
            oldCategory.Description = category.Description;
            oldCategory.Name = category.Name;
            await _context.SaveChangesAsync();
        }

        public async void DeleteCategoryAsync(int id)
        {
            var category = _context.Categories.FirstOrDefault(cat => cat.CategoryId == id);
            if (category == null) return;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public Task<Category> GetCategoryAsync(int id)
        {
            return Task<Category>.Factory.StartNew(() => GetCategory(id));
        }

        public byte[] GetPhysicalFile(int id)
        {
            var file = _context.Files.FirstOrDefault(f => f.FileId == id);
            return (file == null)
                ? null
                : System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + file.FileId +
                                              "_" + file.Name);
        }

        public async Task<byte[]> GetPhysicalFileAsync(int id)
        {
            return await Task<byte[]>.Factory.StartNew(() => GetPhysicalFile(id));
        }

        public IList<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public ApplicationUser GetUserByUserName(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public void EditUser(ApplicationUser newUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == newUser.Id);
            user.UsedSpace = newUser.UsedSpace;
            user.TotalSpace = newUser.TotalSpace;
            _context.SaveChanges();
        }

        public async void EditUserAsync(ApplicationUser newUser)
        {
            await Task.Factory.StartNew(() => EditUser(newUser));
        }

        private void AddPhysicalFile(File file, byte[] fileBytes)
        {
            System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + file.FileId + "_" + file.Name, fileBytes);
        }

        private async void AddPhysicalFileAsync(File file, byte[] fileBytes)
        {
            await Task.Factory.StartNew(() => AddPhysicalFile(file, fileBytes));
        }

        private void DeletePhysicalFile(File file)
        {
            System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + file.FileId + "_" + file.Name);
        }

        private async void DeletePhysicalFileAsync(File file)
        {
            await Task.Factory.StartNew(() => DeletePhysicalFile(file));
        }
    }
}