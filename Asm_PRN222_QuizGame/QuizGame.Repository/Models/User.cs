﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace QuizGame.Repository.Models
{
    public partial class User
    {
        public User()
        {
            Games = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}