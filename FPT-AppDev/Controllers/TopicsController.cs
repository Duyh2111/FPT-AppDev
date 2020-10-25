using FPT_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class TopicsController : Controller
  {
    private readonly ApplicationDbContext _context;
    public TopicsController()
    {
      _context = new ApplicationDbContext();
    }
    [Authorize(Roles = "Staff")]
    public ActionResult Index()
    {
      return View(_context.Topics.ToList());
    }
    [Authorize(Roles = "Staff")]
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Topic topic)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }
      if (_context.Topics.Any(c => c.Name.Contains(topic.Name)))
      {
        ModelState.AddModelError("Name", "Topic Name is already exists.");
        return View(topic);
      }
      _context.Topics.Add(topic);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
    [Authorize(Roles = "Staff")]
    public ActionResult Edit(int id)
    {
      var categoryInDb = _context.Topics
        .SingleOrDefault(p => p.Id == id);
      if (categoryInDb == null)
      {
        return HttpNotFound();
      }
      return View(categoryInDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Topic topic)
    {
      if (_context.Topics.Any(c => c.Name.Contains(topic.Name)))
      {
        ModelState.AddModelError("Name", "Topic Name is already exists.");
        return View(topic);
      }
      var topicInDb = _context.Topics.SingleOrDefault(t => t.Id == topic.Id);
      if (topicInDb == null)
      {
        return HttpNotFound();
      }

      topicInDb.Name = topic.Name;
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpGet]
    [Authorize(Roles = "Staff")]
    public ActionResult Delete(int id)
    {
      var topicInDb = _context.Topics.SingleOrDefault(t => t.Id == id);
      if (topicInDb == null)
      {
        return HttpNotFound();
      }
      _context.Topics.Remove(topicInDb);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
