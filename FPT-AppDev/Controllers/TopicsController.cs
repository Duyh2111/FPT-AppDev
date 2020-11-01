using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class TopicsController : Controller
  {
    private ApplicationDbContext _context;

    public TopicsController()
    {
      _context = new ApplicationDbContext();
    }

    [HttpGet]
    [Authorize(Roles = "Staff")]
    public ActionResult Index(string searchString)
    {
      var topics = _context.Topics
      .Include(c => c.Course);

      if (!String.IsNullOrEmpty(searchString))
      {
        topics = topics.Where(
          s => s.Name.Contains(searchString) ||
          s.Course.Name.Contains(searchString));
      }

      return View(topics.ToList());
    }

    [HttpGet]

    [Authorize(Roles = "Staff")]
    public ActionResult Create()
    {
      var viewModel = new TopicCourseViewModel
      {
        Courses = _context.Courses
        .ToList()
      };
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(Topic topic)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }

      if (_context.Topics.Any(c => c.Name.Contains(topic.Name)))
      {
        ModelState.AddModelError("Name", "Topic Name Already Exists.");
        var viewModel = new TopicCourseViewModel
        {
          Courses = _context.Courses
        .ToList()
        };
        return View(viewModel);
      }

      var newTopic = new Topic
      {
        Name = topic.Name,
        Description = topic.Description,
        CourseId = topic.CourseId,
      };

      _context.Topics.Add(newTopic);
      _context.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Staff")]
    public ActionResult Delete(int id)
    {
      var TopicInDb = _context.Topics.SingleOrDefault(c => c.Id == id);

      if (TopicInDb == null)
      {
        return HttpNotFound();
      }

      _context.Topics.Remove(TopicInDb);
      _context.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Staff")]
    public ActionResult Edit(int id)
    {
      var TopicInDb = _context.Topics.SingleOrDefault(c => c.Id == id);

      if (TopicInDb == null)
      {
        return HttpNotFound();
      }

      var viewModel = new TopicCourseViewModel
      {
        Topic = TopicInDb,
        Courses = _context.Courses
        .ToList()
      };

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(Topic topic)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }

      if (_context.Topics.Any(c => c.Name.Contains(topic.Name)))
      {
        ModelState.AddModelError("Name", "Topic Name Already Exists.");
        var viewModel = new TopicCourseViewModel
        {
          Courses = _context.Courses
        .ToList()
        };

        return View(viewModel);
      }

      var TopicInDb = _context.Topics.SingleOrDefault(c => c.Id == topic.Id);

      if (TopicInDb == null)
      {
        return HttpNotFound();
      }

      TopicInDb.Name = topic.Name;
      TopicInDb.Description = topic.Description;
      TopicInDb.CourseId = topic.CourseId;
      _context.SaveChanges();

      return RedirectToAction("Index");
    }
  }
}