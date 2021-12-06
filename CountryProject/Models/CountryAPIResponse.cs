using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryProject.Models
{
    public class CountryAPIResponse
    {
        public CountryName name { set; get; }
        public string cca2 { get; set; }
    }

    public class CountryName
    {
        public string common { get; set; }
    }

}
