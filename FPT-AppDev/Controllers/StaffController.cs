using FPT_AppDev.Models;
using FPT_AppDev.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace FPT_AppDev.Controllers
{
  public class StaffController : Controller
  {
    private ApplicationDbContext _context;

    public StaffController()
    {
      _context = new ApplicationDbContext();
    }

    [Authorize(Roles = "Staff")]
    public ActionResult Index()
    {
      var role = (from r in _context.Roles where r.Name
                  .Contains("Trainee") select r)
                  .FirstOrDefault();
      var users = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(role.Id))
        .ToList();
      var userVM = users.Select(user => new AccountViewModel
      {
        UserName = user.UserName,
        Email = user.Email,
        RoleName = "Trainee",
        UserId = user.Id
      }).ToList();

      var role2 = (from r in _context.Roles where r.Name
                   .Contains("Trainer") select r)
                   .FirstOrDefault();
      var users2 = _context.Users
        .Where(x => x.Roles
        .Select(y => y.RoleId)
        .Contains(role2.Id))
        .ToList();
      var user2VM = users2.Select(user => new AccountViewModel
      {
        UserName = user.UserName,
        Email = user.Email,
        RoleName = "Trainer",
        UserId = user.Id
      }).ToList();


      var model = new AccountViewModel { Trainee = userVM, Trainer = user2VM };
      return View(model);
    }
    [HttpGet]
    [Authorize(Roles = "Staff")]
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
    [Authorize(Roles = "Staff")]
    public ActionResult Edit(ApplicationUser user)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }
      var UsernameIsAlreadyExist = _context.Users
        .Any(p => p.UserName
        .Contains(user.UserName));

      if (UsernameIsAlreadyExist)
      {
        ModelState.AddModelError("UserName", "Username already existed");
        return View();
      }

      var AccountInDB = _context.Users.SingleOrDefault(p => p.Id == user.Id);

      if (AccountInDB == null)
      {
        return HttpNotFound();

      }

      AccountInDB.UserName = user.UserName;

      _context.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "Staff")]
    public ActionResult Delete(string id)
    {
      var userInDb = _context.Users.SingleOrDefault(p => p.Id == id);

      if (userInDb == null)
      {
        return HttpNotFound();
      }
      _context.Users.Remove(userInDb);
      _context.SaveChanges();

      return RedirectToAction("Index", "Staff");

    }
  }
}
