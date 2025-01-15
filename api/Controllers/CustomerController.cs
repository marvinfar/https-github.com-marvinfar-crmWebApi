using api.Models;
using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class CustomerController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public Customer Get(string id)
        {
            var conn = new CRMHelper.Connection();
            var service = conn.GetCrmService();
            var ent=service.Retrieve("contact", new Guid("{8DF94828-7C97-EF11-B834-005056B60B13}"), new ColumnSet("contactid,fullname,chr_identificationno"));
            var customer = new Customer();
            if (ent != null )
            {
                customer.Id = new Guid(ent["contactid"].ToString());
                customer.IdentificationNo = ent["chr_identificationno"].ToString();
                customer.Type = true;
                customer.Firstname = ent["fullname"].ToString();
                customer.Lastname = ent["fullname"].ToString();
            }
            else
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return customer;
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}