using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPT_AppDev.Models
{
  public class Topic
  {
    public int Id { get; set; }
    [Required]
    [Display(Name = "Topic Name")]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
  }
}