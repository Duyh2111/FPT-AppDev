using FPT_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPT_AppDev.ViewModels
{
  public class TopicCourseViewModel
  {
    public Topic Topic { get; set; }
    public IEnumerable<Course> Courses { get; set; }
  }
}