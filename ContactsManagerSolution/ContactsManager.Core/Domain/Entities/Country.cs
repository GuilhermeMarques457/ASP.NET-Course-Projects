using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for storing the country details
    /// </summary>
    public class Country
    {
        //Guid the values are infinity
        [Key]
        public  Guid CountryID { get; set; }
        [StringLength(40)]
        public string? CountryName { get; set; }

        public virtual ICollection<Person>? People { get; set; }
    }
}