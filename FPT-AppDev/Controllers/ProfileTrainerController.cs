using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class ProfileTrainerController : Controller
  {
    private ApplicationDbContext _context;
    public ProfileTrainerController()
    {
      _context = new ApplicationDbContext();
    }
    // GET: ManageUsers
    [Authorize(Roles = "Trainer")]
    public ActionResult Index()
    {
      var roleTrainer = (from r in _context.Roles
                         where r.Name.Contains("Trainer")
                         select r)
                         .FirstOrDefault();

      var trainers = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(roleTrainer.Id))
        .ToList();

      var trainer = trainers
        .Where(x => x.Id == User.Identity
        .GetUserId())
        .SingleOrDefault();

      var trainerVM = new ProfileTraineeTrainerViewModel
      {
        Email = trainer.Email,
        RoleName = "Trainer",
        UserId = trainer.Id
      };

      var account = new ProfileTraineeTrainerViewModel
      {
        SingleTrainer = trainerVM,
      };
      return View(account);
    }

  }
}