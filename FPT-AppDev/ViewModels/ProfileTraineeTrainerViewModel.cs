namespace FPT_AppDev.ViewModels
{
  public class ProfileTraineeTrainerViewModel
  {
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public ProfileTraineeTrainerViewModel SingleTrainer { get; set; }
    public ProfileTraineeTrainerViewModel SingleTrainee { get; set; }
  }
}