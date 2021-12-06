using CountryProject.Controllers;
using CountryProject.Models;
using HoildaysProject.Data;
using HoildaysProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpDelete("{holidayId}")]
        public IActionResult GetHolidayById(string id)
        {
            try {
                Holiday holiday = _holidayMethods.GetHoliday(id);
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
        public IActionResult DeleteHolidaysForSpecificCountryByCountryName(string countryName, string id)
        {
            try
            {
                _holidayMethods.DeleteHolidaysForSpecificCountryByCountryName(countryName, id);
                return new JsonResult(new { Result = "Deleted" });
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { Error = ex });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }

        [HttpDelete("{holidayId}")]
        public IActionResult DeleteHolidayById(string id)
        {
            try
            {
                _holidayMethods.DeleteHoliday(id);
                return new JsonResult(new { Result = "Deleted" });
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { Error = ex });
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
                return new JsonResult(new { Error = ex });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex.Message });
            }
        }




    }
}
