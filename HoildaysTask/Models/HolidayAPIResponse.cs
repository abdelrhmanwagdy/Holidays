using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoildaysProject.Models
{
    public class HolidayAPIResponse
    {
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string id { set; get; }
        public string summary { set; get; }
        public string description { set; get; }
    }
}
