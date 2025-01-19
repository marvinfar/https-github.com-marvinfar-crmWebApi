using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string emailaddress1 { get; set; }  
        public string mobilephone { get; set; }
    }
    public class InputParametersForGetContact
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
    
}