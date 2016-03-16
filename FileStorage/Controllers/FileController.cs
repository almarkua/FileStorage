using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileStorage.Domain;
using FileStorage.Models;
using WebGrease.Css.Extensions;
using File = FileStorage.Domain.File;

namespace FileStorage.Controllers
{
    [Authorize]
    public class FileController : BaseController
    {

        public FileController(IFilesRepository repository) : base(repository)
        {
        }

        // GET: Files
        public ActionResult Index()
        {
            var allFiles = Repository.GetFiles(null);

            var fileVMs = new List<FileViewModel>();
            Repository.GetFiles(CurrentUser.Id).ForEach(x => fileVMs.Add(new FileViewModel(x)));
            return View(fileVMs);
        }

        public ActionResult Category(int? id) {
            var fileVMs = new List<FileViewModel>();
            if (id == null)
            {
                Repository.GetFiles(CurrentUser.Id).ForEach(x => fileVMs.Add(new FileViewModel(x)));
            }
            else if (id == 0) {
                Repository.GetFiles(CurrentUser.Id).Where(cat => cat.Category == null).ForEach(x => fileVMs.Add(new FileViewModel(x)));
            }
            {
                Repository.GetFiles(CurrentUser.Id).Where(f => f.Category.CategoryId == id).ForEach(x => fileVMs.Add(new FileViewModel(x)));
            }
            return View(fileVMs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var catItems = new List<SelectListItem>();
            CurrentUser.Categories.ForEach(cat => catItems.Add(new SelectListItem() { Text = cat.Name, Value = cat.CategoryId.ToString() }));
            return View(new CreateFileViewModel() { Description = String.Empty, Categories = catItems});
        }

        [HttpPost]
        public ActionResult Create(CreateFileViewModel model, HttpPostedFileBase fileBase)
        {
                if (ModelState.IsValid && fileBase != null && fileBase.ContentLength > 0)
                {
                    byte[] fileBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    fileBase.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                Repository.AddFile(new File()
                {
                    Name = fileBase.FileName,
                    Description = model.Description,
                    ContentType = fileBase.ContentType,
                    Size = fileBase.ContentLength,
                    IsShared = false,
                    Category = CurrentUser.Categories.FirstOrDefault(cat => cat.CategoryId == model.CategoryId),
                    User = CurrentUser
                }, fileBytes );
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public FileContentResult Download(int id)
        {
            var file = Repository.GetFiles(CurrentUser.Id).FirstOrDefault(w => w.FileId == id);
            if (file != null)
            {
                return File(Repository.GetPhysicalFile(file.FileId), file.ContentType, file.Name);
            }
            return null;
        }

        public ActionResult Share(int id)
        {
            var file = Repository.GetFiles(CurrentUser.Id).FirstOrDefault(f => f.FileId == id);
            if (file != null)
            {
                file.IsShared = !file.IsShared;
                Repository.EditFile(file);
                return RedirectToAction("Index");
            }
            return null;
        }

        [AllowAnonymous]
        public FileContentResult GetShared(int id)
        {
            var file = Repository.GetFiles(CurrentUser.Id).FirstOrDefault(w => w.FileId == id);
            if (file != null && file.IsShared)
            {
                return File(System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + file.FileId + "_" + file.Name), file.ContentType, file.Name);
            }
            return null;
        }


        public ActionResult Delete(int id)
        {
            var file = Repository.GetFiles(CurrentUser.Id).FirstOrDefault(f => f.FileId == id);
            if (file != null)
            {
                Repository.DeleteFile(file.FileId);
            }
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult Space()
        {
            return PartialView(new SpaceViewModel() {TotalSpace = Domain.File.GetSizeString(CurrentUser.TotalSpace),
                UsedSpace = Domain.File.GetSizeString(CurrentUser.UsedSpace),
                FreeSpace = Domain.File.GetSizeString(CurrentUser.TotalSpace - CurrentUser.UsedSpace),
                UsedSpacePercent = (int)((double)CurrentUser.UsedSpace/CurrentUser.TotalSpace*100)});
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var file = Repository.GetFile(id);
            var fileEditVM = new EditFileViewModel()
            {
                FileId = id, 
                CategoryId = file.Category.CategoryId,
                Description = file.Description,
                Categories = new SelectList(Repository.GetCategories(CurrentUser.Id),"CategoryId","Name")
            };
            var selectedCategory = fileEditVM.Categories.FirstOrDefault(cat => cat.Value.Equals(file.Category.CategoryId.ToString()));
            if (selectedCategory != null)
                selectedCategory.Selected  = true;
            return View("Edit", fileEditVM);
        }

        public ActionResult Edit(EditFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var file = Repository.GetFile(model.FileId);
                file.Description = model.Description;
                file.Category = Repository.GetCategory(model.CategoryId);
                Repository.EditFile(file);
                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }
    }
}