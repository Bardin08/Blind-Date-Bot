﻿// <auto-generated />
using BlindDateBot.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlindDateBot.Data.Migrations
{
    [DbContext(typeof(SqlServerContext))]
    partial class SqlServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("BlindDateBot.Domain.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id")
                        .UseIdentityColumn();

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("user_gender");

                    b.Property<int>("InterlocutorGender")
                        .HasColumnType("int")
                        .HasColumnName("interlocuter_gender");

                    b.Property<bool>("IsFree")
                        .HasColumnType("bit")
                        .HasColumnName("is_free");

                    b.Property<int>("TelegramId")
                        .HasColumnType("int")
                        .HasColumnName("username_telegram_id");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("user_pkey");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
