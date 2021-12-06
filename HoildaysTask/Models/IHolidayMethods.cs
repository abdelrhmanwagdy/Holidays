using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoildaysProject.Models
{
    interface IHolidayMethods
    {
        public IEnumerable<Holiday> GetHolidaysForClients(int _skip);
        public IEnumerable<Holiday> GetHolidaysForSpecificCountryByCountryId(int countryId);
        public IEnumerable<Holiday> GetHolidaysForSpecificCountryByCountryName(string countryName);
        public Holiday PostHolidaysForSpecificCountryByCountryName(string countryName, Holiday body);
        public void DeleteHolidaysForSpecificCountryByCountryName(string countryName, string id);
        public Holiday UpdateHoliday(string id, Holiday body);

        public IEnumerable<Holiday> GetAllHolidaysForServer();
    }
}
