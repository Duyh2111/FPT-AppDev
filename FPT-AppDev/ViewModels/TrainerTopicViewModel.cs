using FPT_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPT_AppDev.ViewModels
{
  public class TrainerTopicViewModel
  {
    public TrainerTopic TrainerTopic { get; set; }
    public IEnumerable<ApplicationUser> Trainers { get; set; }
    public IEnumerable<Topic> Topics { get; set; }
  }
}