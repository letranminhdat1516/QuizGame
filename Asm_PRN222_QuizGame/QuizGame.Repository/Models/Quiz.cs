﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace QuizGame.Repository.Models;

public partial class Quiz
{
    public int QuizId { get; set; }

    public string QuizName { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}