﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace QuizGame.Repository.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int? QuizId { get; set; }

    public string QuestionText { get; set; }

    public string CorrectAnswer { get; set; }

    public virtual ICollection<QuestionInGame> QuestionInGames { get; set; } = new List<QuestionInGame>();

    public virtual Quiz Quiz { get; set; }
}