using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
        public class Customer
        {
            public bool Type { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string IdentificationNo { get; set; }
            public Guid Id { get; set; }
        }
    public class sag1
    {
        public string Id { get; set; }
        public string name { get; set; }
    }
    
}