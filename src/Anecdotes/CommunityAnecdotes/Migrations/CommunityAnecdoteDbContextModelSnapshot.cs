﻿// <auto-generated />
using Anecdotes.CommunityAnecdotes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Anecdotes.CommunityAnecdotes.Migrations
{
    [DbContext(typeof(CommunityAnecdoteDbContext))]
    partial class CommunityAnecdoteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Anecdotes.CommunityAnecdotes.Data.CommunityAnecdoteData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Anecdote")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Anecdotes", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
