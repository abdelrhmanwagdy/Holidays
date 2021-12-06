using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoildaysProject.Models
{
    public class Holiday
    {
        public string id { set; get; }
        public string summary { set; get; }
        public string description { set; get; }
        public int countryId { set; get; }
    }
}
