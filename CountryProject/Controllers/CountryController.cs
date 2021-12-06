using CountryProject.Data;
using CountryProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountryProject.Controllers
{
    public interface ICountryController
    {
    }
    [Route("api/[controller]")]
    [ApiController]

    public class CountryController : ControllerBase, ICountryController
    {
        private readonly TaskDbContext _context;
        private readonly ICountryMethods _countrymethods;

        public CountryController(TaskDbContext _context, ICountryMethods _countrymethods)
        {
            this._context = _context;
            this._countrymethods = _countrymethods;
        }

        // fetching data  from external api and save reult in task database in countries table
        //if the country name is already exsist while fetching it update it 
        //if the country name is not exsist while fetching it add it 

        [HttpGet("FetchFromAPI")]
        public async Task<IActionResult> GetCountriesFromAPI()
        {
            string staticUrlForCountries = "https://restcountries.com/v3.1/all";
            try
            {
                List<Country> countryList = new List<Country>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(staticUrlForCountries))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            List<CountryAPIResponse> apiResponseList = JsonConvert.DeserializeObject<List<CountryAPIResponse>>(apiResponse);
                            IEnumerable<Country> countriesInDB = GetCountriesForServer();
                            foreach (CountryAPIResponse country in apiResponseList)
                            {
                                if (countriesInDB.Any(exsist => exsist.name == country.name.common))
                                {
                                    //update
                                    var entity = _context.countries.Single(c => c.name == country.name.common);
                                    entity.name = country.name.common;
                                    entity.code = country.cca2;
                                    countryList.Add(entity);
                                }
                                else
                                {
                                    //add new item 
                                    Country entity = new Country();
                                    entity.name = country.name.common;
                                    entity.code = country.cca2;
                                    countryList.Add(entity);
                                    _context.countries.Add(entity);
                                }

                            }
                            _context.SaveChanges();
                            return new JsonResult(new { CountriesList = countryList });

                        }
                        else
                        {
                            Response.StatusCode = StatusCodes.Status400BadRequest;
                            return new JsonResult(new { Error = "Failed to fetch data from countries api" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = ex });
            }
        }

        // GET: api/<CountryController>
        //return countries with pagging 
        // parameter _skip is used for pagging to skip first n of countries
        [HttpGet]
        public IEnumerable<Country> GetCountries(int _skip = 0)
        {
            return _countrymethods.GetCountriesForClients(_skip);
        }

        //return all countries 
        public IEnumerable<Country> GetCountriesForServer()
        {
            return _countrymethods.GetAllCountriesForServer();
        }

        //return country by country name
        public Country GetCountryByName(string name)
        {
            return _countrymethods.GetCountryByName(name);
        }

    }
}
