﻿using System.Collections.Generic;

namespace FPT_AppDev.ViewModels
{
  public class Users_In_Role
  {
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string RoleName { get; set; }
    public List<Users_In_Role> Trainee { get; set; }
    public List<Users_In_Role> Trainer { get; set; }
    public List<Users_In_Role> Staff { get; set; }
    
  }
}