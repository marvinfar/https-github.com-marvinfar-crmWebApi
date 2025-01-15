using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.CRMHelper;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Configuration;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using ActionNameAttribute = System.Web.Http.ActionNameAttribute;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var conn = new Connection();
            var service = conn.GetCrmService();
            var ent = service.Retrieve("contact", new Guid("A76FE889-852B-E811-80CC-005056B6C839"), new ColumnSet("contactid", "chr_identificationno"));
            var customers = new List<Customer>();

            if (ent != null)
            {
                var customer = new Customer
                {
                    Id = new Guid(ent["contactid"].ToString()),
                    IdentificationNo = ent["chr_identificationno"].ToString(),
                    Type = true,
                    Firstname = "aaa",
                    Lastname = "aaaa"
                };
                customers.Add(customer);

                customer = new Customer
                {
                    Id = new Guid(ent["contactid"].ToString()),
                    IdentificationNo = ent["chr_identificationno"].ToString(),
                    Type = true,
                    Firstname = "bb",
                    Lastname = "bb"
                };
                customers.Add(customer);

                customer = new Customer
                {
                    Id = new Guid(ent["contactid"].ToString()),
                    IdentificationNo = ent["chr_identificationno"].ToString(),
                    Type = true,
                    Firstname = "cc",
                    Lastname = "cc"
                };
                customers.Add(customer);
            }
            else
            {
                return NotFound();
            }

            var result = new
            {
                data = customers,
                statusCode = 200,
                succeed = true
            };

            return Ok(result); // برگرداندن شیء نهایی با data و statusCode
        }

        
        [System.Web.Http.HttpGet]
        [Route("GetSag")]
        public IHttpActionResult GetSag(string id)
        {
            var conn = new Connection();
            var service = conn.GetCrmService();
            var query = new QueryExpression()
            {
                ColumnSet=new ColumnSet("contactid", "chr_identificationno"),
                EntityName="contact",
                Criteria =
                {
                    Filters=
                    {
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("chr_identificationno",ConditionOperator.Equal,id)
                            }
                        }
                    }
                }
            };
            var ent = service.RetrieveMultiple(query).Entities.FirstOrDefault();

            var customers = new List<Customer>();

            if (ent != null)
            {
                var customer = new Customer
                {
                    Id = new Guid(ent["contactid"].ToString()),
                    IdentificationNo = ent["chr_identificationno"].ToString(),
                    Type = true,
                    Firstname = "aaa",
                    Lastname = "aaaa"
                };
                customers.Add(customer);

            }
            else
            {
                return NotFound();
            }

            var result = new
            {
                data = customers,
                statusCode = 200,
                succeed = true
            };

            return Ok(result); // برگرداندن شیء نهایی با data و statusCode
        }

        // POST api/values
        [System.Web.Http.HttpPost]
        [Route("PostSag")]
        public IHttpActionResult PostSag([FromBody] List<Customer> customers)
        {
            if (customers == null || customers.Count == 0)
            {
                return BadRequest("Customer list is empty or null.");
            }

            // پردازش داده‌های دریافتی
            foreach (var customer in customers)
            {
                // مثلاً ذخیره‌سازی هر مشتری در پایگاه داده
                Console.WriteLine($"Received Customer: {customer.Firstname} {customer.Lastname}");
            }

            return Ok(new
            {
                message = "Customers processed successfully",
                statusCode = true
            });
        }

        [System.Web.Http.HttpPost]
        [Route("PostCat")]
        public IHttpActionResult PostCat([FromBody] sag1 s)
        {
            if (s == null )
            {
                return BadRequest("sag list is empty or null.");
            }

            // پردازش داده‌های دریافتی
            

            return Ok(new
            {
                message = "sag processed successfully",
                statusCode = true
            });
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
