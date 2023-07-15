using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IPeopleRepository
    {
        Task<Person> AddPerson(Person person);

        Task<List<Person>> GetPersonList();

        Task<Person?> GetPersonById(Guid? PersonId);


        /// <summary>
        /// Returns all people based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching people based on given condition</returns>
        Task<List<Person>> GetFilterdPeople(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Updates the specified person
        /// </summary>
        /// <param name="person">Person details with ID to update</param>
        /// <returns>Returns person response object to update</returns>
        Task<Person> UpdatePerson(Person person);

        /// <summary>
        /// Deletes a person based on given Id
        /// </summary>
        /// <param name="PersonId"></param>
        /// <returns>true if the delete is successfull
        /// otherwise false</returns>
        Task<bool> DeletePerson(Guid PersonId);

    }
}
