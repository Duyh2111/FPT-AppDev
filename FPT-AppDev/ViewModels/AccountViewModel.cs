using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FPT_AppDev.ViewModels
{
  public class AccountViewModel
  {
    public string UserId { get; set; }
    [Required]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public List<AccountViewModel> Trainee { get; set; }
    public List<AccountViewModel> Trainer { get; set; }
    public AccountViewModel SingleTrainer { get; set; }
    public AccountViewModel SingleTrainee { get; set; }

  }
}

