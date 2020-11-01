using System.ComponentModel.DataAnnotations;

namespace FPT_AppDev.Models
{
  public class TraineeCourse
  {
    public int Id { get; set; }
    [Display(Name = "Trainee Name")]
    public string TraineeId { get; set; }
    [Display(Name = "Course Name")]
    public int CourseId { get; set; }
    public ApplicationUser Trainee { get; set; }
    public Course Course { get; set; }
  }
}