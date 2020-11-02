using FPT_AppDev.Models;
using System.ComponentModel.DataAnnotations;

namespace FPT_AppDev.ViewModels
{
  public class Users_In_Role
  {
    public string UserId { get; set; }
    [Required]
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string RoleName { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
  }
}