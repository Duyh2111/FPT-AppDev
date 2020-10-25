using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class AccountViewModelsController : Controller
  {
    // GET: AccountViewModels
    ApplicationDbContext _context;
    public AccountViewModelsController()
    {
      _context = new ApplicationDbContext();
    }
    [Authorize(Roles = "Admin")]
    public ActionResult StaffAccount()
    {
      {
        var roleStaff = (from r in _context.Roles
                         where r.Name.Contains("Staff")
                         select r)
                         .FirstOrDefault();

        var staffs = _context.Users.Where(x => x.Roles
                        .Select(y => y.RoleId)
                        .Contains(roleStaff.Id))
                      .ToList();

        var staffVM = staffs.Select(user => new AccountViewModel
        {
          Email = user.Email,
          PhoneNumber = user.PhoneNumber,
          RoleName = "Staff",
          UserId = user.Id
        }).ToList();
        var roleTrainer = (from r in _context.Roles
                           where r.Name.Contains("Trainer")
                           select r)
                         .FirstOrDefault();

        var trainers = _context.Users
          .Where(x => x.Roles
          .Select(y => y.RoleId)
          .Contains(roleTrainer.Id))
          .ToList();
        var trainerVM = trainers.Select(user => new AccountViewModel
        {
          Email = user.Email,
          RoleName = "Trainer",
          UserId = user.Id
        }).ToList();

        var model = new AccountViewModel
        {
          Staff = staffVM,
          Trainer = trainerVM
        };

        return View(model);
      }
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
      var trainerVM = trainers.Select(user => new AccountViewModel
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

      var traineeVM = trainees.Select(user => new AccountViewModel
      {
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        RoleName = "Trainee",
        UserId = user.Id
      }).ToList();
      var account = new AccountViewModel
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

      var trainerVM = new AccountViewModel
      {
        Email = trainer.Email,
        PhoneNumber = trainer.PhoneNumber,
        RoleName = "Trainer",
        UserId = trainer.Id
      };

      var account = new AccountViewModel
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

      var traineeVM = new AccountViewModel
      {
        Email = trainee.Email,
        PhoneNumber = trainee.PhoneNumber,
        RoleName = "Trainee",
        UserId = trainee.Id
      };

      var account = new AccountViewModel
      {
        SingleTrainee = traineeVM,
      };
      return View(account);
    }
    [HttpGet]
    // [Authorize(Roles = "Admin")]
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
    //[Authorize(Roles = "Admin")]
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
        userInDb.Age = user.Age;
        userInDb.Phone = user.Phone;
        userInDb.Email = user.Email;


        _context.Users.AddOrUpdate(userInDb);
        _context.SaveChanges();

        return RedirectToAction("AccountViewModels");
      }
      return View(user);

    }
  }
}
