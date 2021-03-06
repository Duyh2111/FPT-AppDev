﻿using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class AdminController : Controller
  {
    private ApplicationDbContext _context;
    public AdminController()
    {
      _context = new ApplicationDbContext();
    }
    // GET: ManageUsers
    [Authorize(Roles = "Admin")]
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

    [HttpGet]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
        userInDb.Email = user.Email;


        _context.Users.AddOrUpdate(userInDb);
        _context.SaveChanges();

        return RedirectToAction("UsersWithRoles");
      }

      return View(user);

    }

    [Authorize(Roles = "Admin")]
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