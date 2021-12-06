using CountryProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryProject.Models
{
    public class MySQLCountryMethods : ICountryMethods
    {
        private readonly TaskDbContext _context;
        public readonly int _limit = 50;


        public MySQLCountryMethods(TaskDbContext _context)
        {
            this._context = _context;
        }


        //getting all countries without pagging
        public IEnumerable<Country> GetAllCountriesForServer()
        {
            return _context.countries.ToList();
        }


        //getting countries with pagging
        IEnumerable<Country> ICountryMethods.GetCountriesForClients(int _skip)
        {
            return _context.countries.Skip(_skip).Take(_limit).ToList();
        }


        // get country by name
        public Country GetCountryByName(string name)
        {
            return _context.countries.Where(c => c.name == name).FirstOrDefault();
        }
    }

}
