using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPT_AppDev.Models
{
  public class Trainee
  {
    public int Id { get; set; }
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; }
    [Required]
    public int PhoneNumber { get; set; }
    [Required]
    [Display(Name = "Working Place")]
    public string WorkingPlace { get; set; }
  }
}