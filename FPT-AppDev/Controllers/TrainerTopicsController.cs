using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class TrainerTopicsController : Controller
  {
    private ApplicationDbContext _context;

    public TrainerTopicsController()
    {
      _context = new ApplicationDbContext();
    }
    [Authorize(Roles = "Staff, Trainer")]
    public ActionResult Index()
    {
      if (User.IsInRole("Staff"))
      {
        var trainertopics = _context.TrainerTopics
          .Include(t => t.Topic)
          .Include(t => t.Trainer)
          .ToList();
        return View(trainertopics);
      }

      if (User.IsInRole("Trainer"))
      {
        var trainerId = User.Identity.GetUserId();
        var Res = _context.TrainerTopics
          .Where(e => e.TrainerId == trainerId)
          .Include(t => t.Topic)
          .ToList();
        return View(Res);
      }
      return View("Login");
    }

    [Authorize(Roles = "Staff, Trainer")]
    [HttpGet]
    public ActionResult Create()
    {
      var role = (from r in _context.Roles
                  where r.Name.Contains("Trainer")
                  select r)
                  .FirstOrDefault();
      var users = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(role.Id))
        .ToList();

      var topics = _context.Topics.ToList();

      var TrainerTopicVM = new TrainerTopicViewModel()
      {
        Topics = topics,
        Trainers = users,
        TrainerTopic = new TrainerTopic()
      };

      return View(TrainerTopicVM);
    }

    [HttpPost]
    public ActionResult Create(TrainerTopicViewModel model)
    {
      var role = (from r in _context.Roles
                  where r.Name.Contains("Trainer")
                  select r)
                  .FirstOrDefault();
      var users = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(role.Id))
        .ToList();

      var topics = _context.Topics.ToList();
      if (!ModelState.IsValid)
      {
        return View();
      }

      var TrainerTopicVM = new TrainerTopicViewModel()
      {
        Topics = topics,
        Trainers = users,
        TrainerTopic = new TrainerTopic()
      };
      _context.TrainerTopics.Add(model.TrainerTopic);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}