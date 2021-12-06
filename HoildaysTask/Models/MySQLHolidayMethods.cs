using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoildaysProject.Models
{
    public class MySQLHolidayMethods : IHolidayMethods
    {
        public IEnumerable<Holiday> GetAllHolidaysForServer()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Holiday> GetHolidaysForClients(int _skip)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Holiday> GetHolidaysForSpecificCountryByCountryId(int countryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Holiday> GetHolidaysForSpecificCountryByCountryName(string countryName)
        {
            throw new NotImplementedException();
        }

        public Holiday PostHolidaysForSpecificCountryByCountryName(string countryName, Holiday body)
        {
            throw new NotImplementedException();
        }

        public Holiday UpdateHoliday(string id, Holiday body)
        {
            throw new NotImplementedException();
        }

        public void DeleteHolidaysForSpecificCountryByCountryName(string countryName, string id)
        {
            throw new NotImplementedException();
        }
    }
}
