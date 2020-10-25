using FPT_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPT_AppDev.ViewModels
{
  public class CourseViewModel
  {
    public Course Course { get; set; }
    public IEnumerable<Topic> Topics { get; set; }
    public IEnumerable<Category> Categories { get; set; }
  }
}