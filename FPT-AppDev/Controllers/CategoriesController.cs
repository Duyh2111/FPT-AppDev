using FPT_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class CategoriesController : Controller
  {
    private readonly ApplicationDbContext _context;
    public CategoriesController()
    {
      _context = new ApplicationDbContext();
    }

    // GET: Categories
    [Authorize(Roles = "Staff")]
    public ActionResult Index(string searchString)
    {
      return View(_context.Categories.ToList());
    }
    [Authorize(Roles = "Staff")]
    public ActionResult Create()
    {
      return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Category category)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }
      if (_context.Categories.Any(c => c.Name.Contains(category.Name)))
      {
        ModelState.AddModelError("Name", "Category Name is already exists.");
        return View(category);
      }
      var newcategory = new Category
      {
        Name = category.Name,
        Description = category.Description
      };
      _context.Categories.Add(newcategory);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Staff")]
    public ActionResult Edit(int id)
    {
      var categoryInDb = _context.Categories
        .SingleOrDefault(p => p.Id == id);
      if (categoryInDb == null)
      {
        return HttpNotFound();
      }
      return View(categoryInDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Category category)
    {
      if (_context.Categories.Any(c => c.Name.Contains(category.Name)))
      {
        ModelState.AddModelError("Name", "Category Name is already exists.");
        return View(category);
      }
      var categoryInDb = _context.Categories.SingleOrDefault(p => p.Id == category.Id);
      if (categoryInDb == null)
      {
        return HttpNotFound();
      }

      categoryInDb.Name = category.Name;
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpGet]
    [Authorize(Roles = "Staff")]
    public ActionResult Delete(int id)
    {
      var cateInDb = _context.Categories.SingleOrDefault(c => c.Id == id);
      if (cateInDb == null)
      {
        return HttpNotFound();
      }
      _context.Categories.Remove(cateInDb);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
