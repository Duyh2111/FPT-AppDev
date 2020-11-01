using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class TraineeCoursesController : Controller
  {
    private ApplicationDbContext _context;

    public TraineeCoursesController()
    {
      _context = new ApplicationDbContext();
    }

    public ActionResult Index()
    {
      if (User.IsInRole("Staff"))
      {
        var traineecourses = _context.TraineeCourses
          .Include(t => t.Course)
          .Include(t => t.Trainee)
          .ToList();
        return View(traineecourses);
      }
      if (User.IsInRole("Trainee"))
      {
        var traineeId = User.Identity.GetUserId();
        var Res = _context.TraineeCourses
          .Where(e => e.TraineeId == traineeId)
          .Include(t => t.Course)
          .ToList();
        return View(Res);
      }
      return View("Login");
    }
    [Authorize(Roles = "Staff")]
    public ActionResult Create()
    {
      var role = (from r in _context.Roles
                  where r.Name.Contains("Trainee")
                  select r)
                  .FirstOrDefault();
      var users = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(role.Id))
        .ToList();
      var courses = _context.Courses.ToList();

      var traineeCourse = new TraineeCourseViewModel()
      {
        Courses = courses,
        Trainees = users,
        TraineeCourse = new TraineeCourse()
      };

      return View(traineeCourse);
    }

    [HttpPost]
    [Authorize(Roles = "Staff")]
    public ActionResult Create(TraineeCourseViewModel model)
    {
      var role = (from r in _context.Roles
                  where r.Name.Contains("Trainee")
                  select r)
                  .FirstOrDefault();
      var users = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(role.Id))
        .ToList();
      var courses = _context.Courses.ToList();

      if (!ModelState.IsValid)
      {
        return View();
      }

      var traineeCourses = _context.TraineeCourses.ToList();
      var courseId = model.TraineeCourse.CourseId;

      var checkTraineeInCourse = traineeCourses
        .SingleOrDefault(c => c.CourseId == courseId && c.TraineeId == model.TraineeCourse.TraineeId);

      if (checkTraineeInCourse != null)
      {
        ModelState.AddModelError("Name", "Trainee Course Already Exists.");
        //return RedirectToAction("Create");
        var traineeCourse = new TraineeCourseViewModel()
        {
          Courses = courses,
          Trainees = users,
          TraineeCourse = new TraineeCourse()
        };
        return View(traineeCourse);
      }



      _context.TraineeCourses.Add(model.TraineeCourse);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}