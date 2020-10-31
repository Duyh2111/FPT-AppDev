using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class ProfileTraineeController : Controller
  {
    private ApplicationDbContext _context;
    public ProfileTraineeController()
    {
      _context = new ApplicationDbContext();
    }
    // GET: ProfileTrainee
    [Authorize(Roles = "Trainee")]
    public ActionResult Index()
    {
      var roleTrainee = (from r in _context.Roles
                         where r.Name.Contains("Trainee")
                         select r)
                         .FirstOrDefault();

      var trainees = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(roleTrainee.Id))
        .ToList();

      var trainee = trainees
        .Where(x => x.Id == User.Identity
        .GetUserId())
        .SingleOrDefault();

      var traineeVM = new ProfileTraineeTrainerViewModel
      {
        Email = trainee.Email,
        RoleName = "Trainee",
        UserId = trainee.Id
      };

      var account = new ProfileTraineeTrainerViewModel
      {
        SingleTrainee = traineeVM,
      };
      return View(account);
    }
  }
}