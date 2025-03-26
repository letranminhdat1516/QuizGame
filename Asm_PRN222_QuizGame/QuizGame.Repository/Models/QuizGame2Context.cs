﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuizGame.Repository.Models;

public partial class QuizGame2Context : DbContext
{
    public QuizGame2Context(DbContextOptions<QuizGame2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameLog> GameLogs { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerAnswer> PlayerAnswers { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionInGame> QuestionInGames { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamScore> TeamScores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Game__2AB897DD9260330F");

            entity.ToTable("Game");

            entity.HasIndex(e => e.GamePin, "UQ__Game__0902D2DFE08B40A8").IsUnique();

            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.GameName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GamePin)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HostId).HasColumnName("HostID");
            entity.Property(e => e.QuizId).HasColumnName("QuizID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('Waiting')");

            entity.HasOne(d => d.Host).WithMany(p => p.Games)
                .HasForeignKey(d => d.HostId)
                .HasConstraintName("FK__Game__HostID__52593CB8");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Games)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK__Game__QuizID__534D60F1");
        });

        modelBuilder.Entity<GameLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__GameLog__5E5499A81A2AF3F9");

            entity.ToTable("GameLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ActionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GameId).HasColumnName("GameID");

            entity.HasOne(d => d.Game).WithMany(p => p.GameLogs)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__GameLog__GameID__6FE99F9F");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK__Player__4A4E74A8DAB7EE36");

            entity.ToTable("Player");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.JoinTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("JOIN_TIME");
            entity.Property(e => e.PinCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Players)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Player__GameID__628FA481");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__Player__TeamID__6383C8BA");

            entity.HasOne(d => d.User).WithMany(p => p.Players)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Player__UserID__619B8048");
        });

        modelBuilder.Entity<PlayerAnswer>(entity =>
        {
            entity.HasKey(e => e.PlayerAnswerId).HasName("PK__PlayerAn__B300DB8CC3EFF9A1");

            entity.ToTable("PlayerAnswer");

            entity.Property(e => e.PlayerAnswerId).HasColumnName("PlayerAnswerID");
            entity.Property(e => e.Answer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.QuestionInGameId).HasColumnName("QuestionInGameID");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerAnswers)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("FK__PlayerAns__Playe__66603565");

            entity.HasOne(d => d.QuestionInGame).WithMany(p => p.PlayerAnswers)
                .HasForeignKey(d => d.QuestionInGameId)
                .HasConstraintName("FK__PlayerAns__Quest__6754599E");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8CF1489D6F");

            entity.ToTable("Question");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.QuestionText)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.QuizId).HasColumnName("QuizID");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK__Question__QuizID__5629CD9C");
        });

        modelBuilder.Entity<QuestionInGame>(entity =>
        {
            entity.HasKey(e => e.QuestionInGameId).HasName("PK__Question__BFABBBA6674AEDAF");

            entity.ToTable("QuestionInGame");

            entity.Property(e => e.QuestionInGameId).HasColumnName("QuestionInGameID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.Game).WithMany(p => p.QuestionInGames)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__QuestionI__GameI__59FA5E80");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionInGames)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__QuestionI__Quest__59063A47");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PK__Quiz__8B42AE6E8E94DDDE");

            entity.ToTable("Quiz");

            entity.Property(e => e.QuizId).HasColumnName("QuizID");
            entity.Property(e => e.QuizName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__Team__123AE7B9C2BAB1CF");

            entity.ToTable("Team");

            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.TeamName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Game).WithMany(p => p.Teams)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Team__GameID__5DCAEF64");
        });

        modelBuilder.Entity<TeamScore>(entity =>
        {
            entity.HasKey(e => e.TeamScoreId).HasName("PK__TeamScor__0B675FA902568C65");

            entity.ToTable("TeamScore");

            entity.Property(e => e.TeamScoreId).HasColumnName("TeamScoreID");
            entity.Property(e => e.QuestionInGameId).HasColumnName("QuestionInGameID");
            entity.Property(e => e.Score).HasDefaultValueSql("((0))");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.QuestionInGame).WithMany(p => p.TeamScores)
                .HasForeignKey(d => d.QuestionInGameId)
                .HasConstraintName("FK__TeamScore__Quest__6C190EBB");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamScores)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__TeamScore__TeamI__6B24EA82");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACBA5E97E9");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534D74C449D").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}