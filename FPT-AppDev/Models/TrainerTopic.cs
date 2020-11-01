using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPT_AppDev.Models
{
  public class TrainerTopic
  {
    public int Id { get; set; }
    [Display(Name = "Trainer Name")]
    public string TrainerId { get; set; }
    [Display(Name = "Topic Name")]
    public int TopicId { get; set; }
    public ApplicationUser Trainer { get; set; }
    public Topic Topic { get; set; }
  }
}