using CountryProject.Models;
using HoildaysProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoildaysProject.Models
{
    public class MySQLHolidayMethods : IHolidayMethods
    {

        private readonly TaskDbContext _context;
        public readonly int _limit = 50;

        public MySQLHolidayMethods(TaskDbContext _context)
        {
            this._context = _context;
        }



        public IEnumerable<Holiday> GetAllHolidaysForServer()
        {
            return _context.holidays.ToList();
        }


        public IEnumerable<Holiday> GetHolidaysForClients(int _skip)
        {
            return _context.holidays.Skip(_skip).Take(_limit).ToList();
        }


        public Holiday GetHoliday(string id)
        {
            return _context.holidays.Where(h => h.id == id).FirstOrDefault();
        }



        public Holiday PostHolidaysForSpecificCountryByCountryId(int countryId, Holiday body)
        {
            Country country = _context.countries.Where(c => c.id == countryId).FirstOrDefault();
            if (country == null)
            {
                throw new ArgumentException("Please enter the correct country id");
            }

            Holiday holiday = new Holiday();
            holiday.id = Guid.NewGuid().ToString();
            holiday.countryId = country.id;
            holiday.summary = body.summary;
            holiday.description = body.description;
            _context.holidays.Add(holiday);
            _context.SaveChanges();
            return holiday;
        }



        public Holiday PostHolidaysForSpecificCountryByCountryName(string countryName, Holiday body)
        {
            Country country = _context.countries.Where(c => c.name == countryName).FirstOrDefault();
            if (country == null)
            {
                throw new ArgumentException("Please enter the correct country name");
            }

            Holiday holiday = new Holiday();
            holiday.id = Guid.NewGuid().ToString();
            holiday.countryId = country.id;
            holiday.summary = body.summary;
            holiday.description = body.description;
            _context.holidays.Add(holiday);
            _context.SaveChanges();
            return holiday;
        }



        public Holiday UpdateHoliday(string id, Holiday body)
        {
            Holiday holiday = _context.holidays.Where(h => h.id == id).FirstOrDefault();
            if (holiday == null)
            {
                throw new ArgumentException("Please enter the correct holiday id");
            }
            if (body.countryId != 0)
            {
                Country country = _context.countries.Where(c => c.id == body.countryId).FirstOrDefault();
                if (country == null)
                {
                    throw new ArgumentException("Please enter the correct country id");
                }
                holiday.countryId = body.countryId;
            }
            if (body.summary != null)
            {
                holiday.summary = body.summary;
            }
            if (body.description != null)
            {
                holiday.description = body.description;
            }

            _context.SaveChanges();
            return holiday;
        }



        public void DeleteHolidaysForSpecificCountryByCountryName(string countryName, string id)
        {
            Country country = _context.countries.Where(c => c.name == countryName).FirstOrDefault();
            if (country == null)
            {
                throw new ArgumentException("Please enter the correct country name");
            }
            Holiday holiday = _context.holidays.Where(h => h.id == id && h.countryId == country.id).FirstOrDefault();
            if (holiday == null)
            {
                throw new ArgumentException("Please enter the correct holiday id");
            }
            _context.holidays.Remove(holiday);
            _context.SaveChanges();
        }



        public void DeleteHoliday(string id)
        {
            Holiday holiday = _context.holidays.Where(h => h.id == id ).FirstOrDefault();
            if (holiday == null)
            {
                throw new ArgumentException("Please enter the correct holiday id");
            }
            _context.holidays.Remove(holiday);
            _context.SaveChanges();
        }


    }
}
