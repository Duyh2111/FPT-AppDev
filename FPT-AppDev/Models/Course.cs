﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPT_AppDev.Models
{
  public class Course
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
  }
}