using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class OfficeContactCollection
    {
        // this is a list of all the contacts returned by the json and the link for the next cluster
        public List<OfficeContact> value { get; set; }
        public string nextLink { get; set; }
    }
}
