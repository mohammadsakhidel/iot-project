﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrackDataAccess.Database;

namespace TrackDataAccess.Migrations
{
    [DbContext(typeof(TrackDbContext))]
    partial class TrackDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(256)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TrackDataAccess.Models.AccessCode", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("access_codes");
                });

            modelBuilder.Entity("TrackDataAccess.Models.CommandLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time")
                        .HasColumnType("datetime");

                    b.Property<string>("Payload")
                        .HasColumnName("payload")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Response")
                        .HasColumnName("response")
                        .HasColumnType("varchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("TrackerId")
                        .IsRequired()
                        .HasColumnName("tracker_id")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnName("type")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("command_logs");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Identity.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DeleteTime")
                        .HasColumnName("delete_time")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Explanation")
                        .HasColumnName("explanation")
                        .HasColumnType("varchar(512)")
                        .HasMaxLength(512);

                    b.Property<bool>("IsActive")
                        .HasColumnName("is_active")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Image", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnName("bytes")
                        .HasColumnType("MEDIUMBLOB");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time")
                        .HasColumnType("datetime");

                    b.Property<int>("Height")
                        .HasColumnName("height")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("Width")
                        .HasColumnName("width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("images");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time")
                        .HasColumnType("datetime");

                    b.Property<string>("TrackerId")
                        .IsRequired()
                        .HasColumnName("tracker_id")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<string>("discriminator")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("TrackerId");

                    b.ToTable("messages");

                    b.HasDiscriminator<string>("discriminator").HasValue("message");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Tracker", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<string>("CommandSet")
                        .IsRequired()
                        .HasColumnName("command_set")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time")
                        .HasColumnType("datetime");

                    b.Property<string>("DefaultIcon")
                        .HasColumnName("default_icon")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("DeleteTime")
                        .HasColumnName("delete_time")
                        .HasColumnType("datetime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnName("display_name")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Explanation")
                        .HasColumnName("explanation")
                        .HasColumnType("varchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("IconImageId")
                        .HasColumnName("icon_image_id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastConnectedServer")
                        .HasColumnName("last_connected_server")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("LastConnection")
                        .HasColumnName("last_connection")
                        .HasColumnType("datetime");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnName("manufacturer")
                        .HasColumnType("varchar(8)")
                        .HasMaxLength(8);

                    b.Property<string>("ProductModel")
                        .IsRequired()
                        .HasColumnName("product_model")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("ProductType")
                        .IsRequired()
                        .HasColumnName("product_type")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("RawID")
                        .IsRequired()
                        .HasColumnName("raw_id")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnName("serial_number")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Status")
                        .HasColumnName("status")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<string>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("trackers");
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerPermittedUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("TrackerId")
                        .HasColumnName("tracker_id")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnName("permissions")
                        .HasColumnType("varchar(1024)")
                        .HasMaxLength(1024);

                    b.HasKey("UserId", "TrackerId");

                    b.HasIndex("TrackerId");

                    b.ToTable("tracker_permitted_users");
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("TrackerId")
                        .HasColumnName("tracker_id")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("UserId", "TrackerId");

                    b.HasIndex("TrackerId");

                    b.ToTable("tracker_users");
                });

            modelBuilder.Entity("TrackDataAccess.Models.GpsTrackerMessage", b =>
                {
                    b.HasBaseType("TrackDataAccess.Models.Message");

                    b.Property<double?>("Altitude")
                        .HasColumnName("altitude")
                        .HasColumnType("double");

                    b.Property<double?>("Battery")
                        .HasColumnName("battery")
                        .HasColumnType("double");

                    b.Property<double>("Direction")
                        .HasColumnName("direction")
                        .HasColumnType("double");

                    b.Property<bool>("IsValid")
                        .HasColumnName("is_valid")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Latitude")
                        .HasColumnName("latitude")
                        .HasColumnType("double");

                    b.Property<string>("LatitudeMark")
                        .IsRequired()
                        .HasColumnName("latitude_mark")
                        .HasColumnType("varchar(1)")
                        .HasMaxLength(1);

                    b.Property<double>("Longitude")
                        .HasColumnName("longitude")
                        .HasColumnType("double");

                    b.Property<string>("LongitudeMark")
                        .IsRequired()
                        .HasColumnName("longitude_mark")
                        .HasColumnType("varchar(1)")
                        .HasMaxLength(1);

                    b.Property<DateTime>("MessageTime")
                        .HasColumnName("message_time")
                        .HasColumnType("datetime");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnName("message_type")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<double?>("SignalStrength")
                        .HasColumnName("signal_strength")
                        .HasColumnType("double");

                    b.Property<double>("Speed")
                        .HasColumnName("speed")
                        .HasColumnType("double");

                    b.Property<string>("TrackerState")
                        .HasColumnName("tracker_state")
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.ToTable("gps_tracker_messages");

                    b.HasDiscriminator().HasValue("gps_message");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TrackDataAccess.Models.Message", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Tracker", "Tracker")
                        .WithMany()
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerPermittedUser", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Tracker", "Tracker")
                        .WithMany("PermittedUsers")
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", "User")
                        .WithMany("PermittedTrackers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerUser", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Tracker", "Tracker")
                        .WithMany("Users")
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", "User")
                        .WithMany("Trackers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
