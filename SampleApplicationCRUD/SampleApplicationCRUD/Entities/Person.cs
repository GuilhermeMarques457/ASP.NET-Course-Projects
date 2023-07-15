using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)]
        public string? PersonName { get; set; }

        [StringLength(60)]
        public string? PersonEmail { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? PersonGender { get; set; }

        public Guid? CountryID { get; set; }

        [StringLength(200)]
        public string? PersonAddress { get; set; }

        public bool? ReceiveNewsLetters { get; set; }

        [ForeignKey("CountryID")]
        public Country? Country { get; set; }

        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {PersonEmail}, Person Gender: {PersonGender}, Person Address: {PersonAddress}, Receive News Letters: {ReceiveNewsLetters}, Country: {Country}";
        }

    }
}
