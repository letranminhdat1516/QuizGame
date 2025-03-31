﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace QuizGame.Repository.Models
{
    public partial class Player
    {
        public Player()
        {
            PlayerAnswers = new HashSet<PlayerAnswer>();
        }

        public int PlayerId { get; set; }
        public int? UserId { get; set; }
        public int? GameId { get; set; }
        public int? TeamId { get; set; }
        public string PinCode { get; set; }
        public DateTime? JoinTime { get; set; }
        public string PlayerName { get; set; }

        public virtual Game Game { get; set; }
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<PlayerAnswer> PlayerAnswers { get; set; }
    }
}