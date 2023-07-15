﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(AspNetDbContext))]
    [Migration("20230610124323_InsertPerson_StoredProcedure")]
    partial class InsertPerson_StoredProcedure
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("CountryID");

                    b.ToTable("Contries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryID = new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"),
                            CountryName = "Philippines"
                        },
                        new
                        {
                            CountryID = new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"),
                            CountryName = "Thailand"
                        },
                        new
                        {
                            CountryID = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            CountryName = "China"
                        },
                        new
                        {
                            CountryID = new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                            CountryName = "Palestinian Territory"
                        },
                        new
                        {
                            CountryID = new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"),
                            CountryName = "China"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CountryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("PersonAddress")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PersonEmail")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PersonGender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool?>("ReceiveNewsLetters")
                        .HasColumnType("bit");

                    b.HasKey("PersonID");

                    b.ToTable("People", (string)null);

                    b.HasData(
                        new
                        {
                            PersonID = new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"),
                            CountryID = new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"),
                            DateOfBirth = new DateTime(1989, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "4 Parkside Point",
                            PersonEmail = "mwebsdale0@people.com.cn",
                            PersonGender = "Female",
                            PersonName = "Marguerite",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"),
                            CountryID = new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"),
                            DateOfBirth = new DateTime(1990, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "6 Morningstar Circle",
                            PersonEmail = "ushears1@globo.com",
                            PersonGender = "Female",
                            PersonName = "Ursa",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"),
                            CountryID = new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"),
                            DateOfBirth = new DateTime(1995, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "73 Heath Avenue",
                            PersonEmail = "fbowsher2@howstuffworks.com",
                            PersonGender = "Male",
                            PersonName = "Franchot",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"),
                            CountryID = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1987, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "83187 Merry Drive",
                            PersonEmail = "asarvar3@dropbox.com",
                            PersonGender = "Male",
                            PersonName = "Angie",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"),
                            CountryID = new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"),
                            DateOfBirth = new DateTime(1995, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "50467 Holy Cross Crossing",
                            PersonEmail = "ttregona4@stumbleupon.com",
                            PersonGender = "Gender",
                            PersonName = "Tani",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"),
                            CountryID = new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                            DateOfBirth = new DateTime(1988, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "97570 Raven Circle",
                            PersonEmail = "mlingfoot5@netvibes.com",
                            PersonGender = "Male",
                            PersonName = "Mitchael",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("29339209-63f5-492f-8459-754943c74abf"),
                            CountryID = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1983, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "57449 Brown Way",
                            PersonEmail = "mjarrell6@wisc.edu",
                            PersonGender = "Male",
                            PersonName = "Maddy",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"),
                            CountryID = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1998, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "4 Stuart Drive",
                            PersonEmail = "pretchford7@virginia.edu",
                            PersonGender = "Female",
                            PersonName = "Pegeen",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("012107df-862f-4f16-ba94-e5c16886f005"),
                            CountryID = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1990, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "413 Sachtjen Way",
                            PersonEmail = "hmosco8@tripod.com",
                            PersonGender = "Male",
                            PersonName = "Hansiain",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"),
                            CountryID = new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                            DateOfBirth = new DateTime(1997, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "484 Clarendon Court",
                            PersonEmail = "lwoodwing9@wix.com",
                            PersonGender = "Male",
                            PersonName = "Lombard",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"),
                            CountryID = new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"),
                            DateOfBirth = new DateTime(1990, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "2 Warrior Avenue",
                            PersonEmail = "mconachya@va.gov",
                            PersonGender = "Female",
                            PersonName = "Minta",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"),
                            CountryID = new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"),
                            DateOfBirth = new DateTime(1987, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PersonAddress = "9334 Fremont Street",
                            PersonEmail = "vklussb@nationalgeographic.com",
                            PersonGender = "Female",
                            PersonName = "Verene",
                            ReceiveNewsLetters = true
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
