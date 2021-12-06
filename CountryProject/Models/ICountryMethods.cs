using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryProject.Models
{
    public interface ICountryMethods
    {
        //getting all countries from the databse for users (with pagination)
        public IEnumerable<Country> GetCountriesForClients(int _skip);
        //getting all countries from the databse for server (without pagination)
        public IEnumerable<Country> GetAllCountriesForServer();
        public Country GetCountryByName(string name);
    }

}
