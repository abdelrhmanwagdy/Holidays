using CountryProject.Controllers;
using CountryProject.Models;
using HoildaysProject.Data;
using HoildaysProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HoildaysProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class HolidayController : ControllerBase
    {
        private readonly CountryController _countryController;
        private readonly TaskDbContext _context;
        private readonly IHolidayMethods _holidayMethods;

        public HolidayController(TaskDbContext _context, IHolidayMethods _holidayMethods, CountryController _countryController)
        {
            this._holidayMethods = _holidayMethods;
            this._context = _context;
            this._countryController = _countryController;
        }


        [HttpGet("Sync")]
        public async Task<IActionResult> Sync()
        {
            List<Holiday> holidaysList = new List<Holiday>();
            try
            {
                IEnumerable<Country> dbCountryList = _countryController.GetCountriesForServer();
                using (var httpClient = new HttpClient())
                {
                    if (dbCountryList.Any())
                    {
                        foreach (Country country in dbCountryList)
                        {
                            using (var response = await httpClient.GetAsync("https://www.googleapis.com/calendar/v3/calendars/en." + country.code + "%23holiday%40group.v.calendar.google.com/events?key=AIzaSyBpSZoCr4xUGsNzmAuxVw_WT0Q4hVW9Bos"))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    string apiResponse = await response.Content.ReadAsStringAsync();
                                    HolidayAPIResponse apiResponseItem = JsonConvert.DeserializeObject<HolidayAPIResponse>(apiResponse);
                                    IEnumerable<Holiday> holidaysInDB = GetAllHolidaysForServer();
                                    foreach (Item holiday in apiResponseItem.items)
                                    {
                                        //if we want to remove the user inserted holidays through our api we should delete country holidays before starting 
                                        if (holidaysInDB.Any(a => a.id == holiday.id))
                                        {
                                            //update existing holiday
                                            var entity = _context.holidays.Single(c => c.id == holiday.id);
                                            entity.summary = holiday.summary;
                                            entity.description = holiday.description;
                                        }
                                        else
                                        {
                                            //add new one
                                            Holiday entity = new Holiday();
                                            entity.id = holiday.id;
                                            entity.countryId = country.id;
                                            entity.summary = holiday.summary;
                                            entity.description = holiday.description;
                                            _context.holidays.Add(entity);
                                        }
                                    }
                                    _context.SaveChanges();
                                }
                            }
                        }
                        return new JsonResult(new { Result = "Syncronization done between Countries and Holidays" });
                    }
                    else
                    {
                        Response.StatusCode = StatusCodes.Status404NotFound;
                        return new JsonResult(new { Error = "There is no country to fetch their holidays" });
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex });
            }
        }

        //for server
        public Country getCountryByName(string country)
        {
            return _countryController.GetCountryByName(country);
        }

        //for server
        public IEnumerable<Holiday> GetAllHolidaysForServer()
        {
            return _holidayMethods.GetAllHolidaysForServer();
        }


        [HttpGet("countryName/{countryName}")]
        public IActionResult GetHolidaysForSpecificCountryByCountryName(string countryName)
        {
            try
            {
                IEnumerable<Holiday> holidays = _holidayMethods.GetHolidaysForSpecificCountryByCountryName(countryName).ToList();
                return new JsonResult(new { Result = holidays });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }

        [HttpGet("countryId/{countryId}")]
        public IActionResult GetHolidaysForSpecificCountryByCountryId(int countryId)
        {
            try
            {
                IEnumerable<Holiday> holidays = _holidayMethods.GetHolidaysForSpecificCountryByCountryId(countryId).ToList();
                return new JsonResult(new { Result = holidays });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }

        public IEnumerable<Holiday> GetHolidaysForSpecificCountryByCountryIdForServer(int countryId)
        {
                return  _holidayMethods.GetHolidaysForSpecificCountryByCountryId(countryId).ToList();
        }


        [HttpGet("{holidayId}")]
        public IActionResult GetHolidayById(string holidayId)
        {
            try {
                Holiday holiday = _holidayMethods.GetHoliday(holidayId);
                if (holiday == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return new JsonResult(new { Error = "No Holidays with the specified id exists" });
                }
                else
                {
                    return new JsonResult(new { Holiday = holiday });
                }
            }
            catch (Exception ex) 
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }


        [HttpDelete("{countryName}/{holidayId}")]
        public IActionResult DeleteHolidaysForSpecificCountryByCountryName(string countryName, string holidayId)
        {
            try
            {
                _holidayMethods.DeleteHolidaysForSpecificCountryByCountryName(countryName, holidayId);
                return new JsonResult(new { Result = "Deleted" });
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }

        [HttpDelete("{holidayId}")]
        public IActionResult DeleteHolidayById(string holidayId)
        {
            try
            {
                _holidayMethods.DeleteHoliday(holidayId);
                return new JsonResult(new { Result = "Deleted" });
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }


        [HttpPut("{holidayId}")]
        public IActionResult UpdateHoliday(string holidayId, [FromBody] Holiday body)
        {
            try
            {
                Holiday holiday = _holidayMethods.UpdateHoliday(holidayId, body);
                return new JsonResult(new { UpdatedHoliday = holiday });
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }

    }
}
