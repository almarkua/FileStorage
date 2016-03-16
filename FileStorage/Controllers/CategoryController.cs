using FileStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileStorage.Domain;

namespace FileStorage.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        public CategoryController(IFilesRepository repository) : base(repository)
        {
        }
        // GET: Category
        public ActionResult Index()
        {
            return View(CurrentUser.Categories.AsEnumerable());
        }

        [ChildActionOnly]
        public ActionResult AllChild()
        {
            return PartialView(CurrentUser.Categories.GetEnumerator());
        }

        [HttpGet]
        public ActionResult Create() {
            return View(new CreateCategoryViewModel());
        }

        [HttpPost]
        public ActionResult Create(CreateCategoryViewModel model) {
            if (ModelState.IsValid)
            {
                Repository.AddCategory(new Category() { Name = model.Name, Description = model.Description, User = CurrentUser});
                return RedirectToAction("Index");
                ;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = Repository.GetCategory(id);
            var catVM = new EditCategoryViewModel()
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
            return View(catVM);
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel catVM)
        {
            if (ModelState.IsValid)
            {
                var category = Repository.GetCategory(catVM.CategoryId);
                category.Name = catVM.Name;
                category.Description = catVM.Description;
                Repository.EditCategory(category);
                return RedirectToAction("Index");
            }
            return View(catVM);
        }

        public ActionResult Delete(int id)
        {
            Repository.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}