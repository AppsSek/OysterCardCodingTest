﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LondonTransportFareSystem.Models;

public partial class LondonTransportContext : DbContext
{
    public LondonTransportContext(DbContextOptions<LondonTransportContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerTransaction> CustomerTransactions { get; set; }

    public virtual DbSet<Mode> Modes { get; set; }

    public virtual DbSet<ModeBu> ModeBus { get; set; }

    public virtual DbSet<ModeMaxFare> ModeMaxFares { get; set; }

    public virtual DbSet<ModeTube> ModeTubes { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<TubeZone> TubeZones { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.Balance).HasColumnType("money");
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<CustomerTransaction>(entity =>
        {
            entity.ToTable("CustomerTransaction");

            entity.Property(e => e.CurrentBalance).HasColumnType("money");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Debit).HasColumnType("money");
            entity.Property(e => e.Decription).IsRequired();
        });

        modelBuilder.Entity<Mode>(entity =>
        {
            entity.ToTable("Mode");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.MaxFare).HasColumnType("money");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TableName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<ModeBu>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<ModeMaxFare>(entity =>
        {
            entity.ToTable("ModeMaxFare");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.MaxFare).HasColumnType("money");
            entity.Property(e => e.Mode)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<ModeTube>(entity =>
        {
            entity.ToTable("ModeTube");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.ToTable("Station");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.StationName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<TubeZone>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.MinFare).HasColumnType("money");
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e._2).HasColumnType("money");
            entity.Property(e => e._3).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}