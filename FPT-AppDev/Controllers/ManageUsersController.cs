using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class ManageUsersController : Controller
  {
    private ApplicationDbContext _context;
    public ManageUsersController()
    {
      _context = new ApplicationDbContext();
    }
    // GET: ManageUsers
    public ActionResult UsersWithRoles()
    {
      var usersWithRoles = (from user in _context.Users
                            select new
                            {
                              UserId = user.Id,
                              Username = user.UserName,
                              Emailaddress = user.Email,
                              Password = user.PasswordHash,
                              RoleNames = (from userRole in user.Roles
                                           join role in _context.Roles on userRole.RoleId
                                           equals role.Id
                                           select role.Name).ToList()
                            }).ToList().Select(p => new Users_In_Role()

                            {
                              UserId = p.UserId,
                              Username = p.Username,
                              Email = p.Emailaddress,
                              Role = string.Join(",", p.RoleNames)
                            });


      return View(usersWithRoles);
    }
    public ActionResult TrainerTraineeAccount()
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
      var trainerVM = trainers.Select(user => new Users_In_Role
      {
        Email = user.Email,
        RoleName = "Trainer",
        UserId = user.Id
      }).ToList();
      var roleTrainee = (from r in _context.Roles
                         where r.Name.Contains("Trainee")
                         select r)
                         .FirstOrDefault();

      var trainees = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(roleTrainee.Id))
        .ToList();

      var traineeVM = trainees.Select(user => new Users_In_Role
      {
        Email = user.Email,
        RoleName = "Trainee",
        UserId = user.Id
      }).ToList();
      var account = new Users_In_Role
      {
        Trainer = trainerVM,
        Trainee = traineeVM
      };
      return View(account);
    }
    public ActionResult TrainerAccount()
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

      var trainerVM = new Users_In_Role
      {
        Email = trainer.Email,
        RoleName = "Trainer",
        UserId = trainer.Id
      };

      var account = new Users_In_Role
      {
        SingleTrainer = trainerVM,
      };
      return View(account);
    }
    public ActionResult TraineeAccount()
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

      var traineeVM = new Users_In_Role
      {
        Email = trainee.Email,
        RoleName = "Trainee",
        UserId = trainee.Id
      };

      var account = new Users_In_Role
      {
        SingleTrainee = traineeVM,
      };
      return View(account);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
    public ActionResult Edit(string id)
    {
      if (id == null)
      {
        return HttpNotFound();
      }
      var appUser = _context.Users.Find(id);
      if (appUser == null)
      {
        return HttpNotFound();
      }
      return View(appUser);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Staff")]
    public ActionResult Edit(ApplicationUser user)
    {
      var userInDb = _context.Users.Find(user.Id);

      if (userInDb == null)
      {
        return View(user);
      }

      if (ModelState.IsValid)
      {
        userInDb.UserName = user.UserName;
        userInDb.Phone = user.Phone;
        userInDb.Email = user.Email;


        _context.Users.AddOrUpdate(userInDb);
        _context.SaveChanges();

        return RedirectToAction("UsersWithRoles");
      }
      return View(user);

    }

    [Authorize(Roles = "Admin, Staff")]
    public ActionResult Delete(ApplicationUser user)
    {
      var userInDb = _context.Users.Find(user.Id);

      if (userInDb == null)
      {
        return View(user);
      }

      if (ModelState.IsValid)
      {
        userInDb.UserName = user.UserName;
        userInDb.Age = user.Age;
        userInDb.Phone = user.Phone;
        userInDb.Email = user.Email;

        _context.Users.Remove(userInDb);
        _context.SaveChanges();

        return RedirectToAction("UsersWithRoles");
      }
      return View(user);
    }
  }
}