using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    // Hey IdentityDbContext create dbSet for ApplicationUser and for ApplicationRole
    public class AspNetDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        public AspNetDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("People");
            

            //Seed to Countries

            string contries = System.IO.File.ReadAllText("countries.json");
            List<Country>? countriesList = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(contries);

            foreach(var item in countriesList!)
            {
                modelBuilder.Entity<Country>().HasData(item);
            }

            //end

            //Seed to People

            string people = System.IO.File.ReadAllText("persons.json");
            List<Person>? peopleList = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(people);

            foreach (var item in peopleList!)
            {
                modelBuilder.Entity<Person>().HasData(item);
            }

            //end

            //Fluent API

            modelBuilder.Entity<Person>().Property(p => p.PersonGender)
                .HasColumnName("PersonGender")
                .HasColumnType("varchar(10)")
                .HasDefaultValue("Other");

            //end

            //Table Relations

            //modelBuilder.Entity<Person>(p =>
            //{
            //    p.HasOne<Country>(c => c.Country)
            //    .WithMany(p => p.People)
            //    .HasForeignKey(p => p.CountryID);
            //});

            //end
        }

        public List<Person> sp_GetAllPeople()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPeopleRight]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@PersonEmail", person.PersonEmail),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@PersonGender", person.PersonGender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@PersonAddress", person.PersonAddress),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
            };

            //Execute sqlraw returns a int type (numbers of rows affected)
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson]" +
                "@PersonID, @PersonName, @PersonEmail, @DateOfBirth, @PersonGender," +
                "@CountryID, @PersonAddress, @ReceiveNewsLetters", parameters);
        }
    }
}
