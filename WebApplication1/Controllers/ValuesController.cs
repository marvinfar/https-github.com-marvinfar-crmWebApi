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
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using ActionNameAttribute = System.Web.Http.ActionNameAttribute;
using Microsoft.Xrm.Sdk;
using System.Web.Http.ModelBinding;
using System.Diagnostics.Contracts;
using System.CodeDom;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.Http.Controllers;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {

        private bool ValidateHeaders(HttpRequestMessage request)
        {
            if (!request.Headers.Contains("AppName") || !request.Headers.Contains("Token"))// || !request.Headers.Contains("Authorization"))
            {
                return false;
            }

            var appName = request.Headers.GetValues("AppName").FirstOrDefault();
            var token = request.Headers.GetValues("Token").FirstOrDefault();
           // var authorization = request.Headers.GetValues("Authorization").FirstOrDefault();

          
            if (appName != "MyApp" || token != "123456" )//|| authorization != "Bearer xyz")
            {
                return false;
            }

            return true;
        }

        [System.Web.Http.HttpGet]
        [Route("{id:guid}")]
        public IHttpActionResult Get(Guid id)
        {
            if (!ValidateHeaders(Request))
            {
                return (IHttpActionResult)Unauthorized(); // خطای 401 برمی‌گرداند
            }
            
            Contact contact = null;
            object result = null;
            try
            {
                var conn = new Connection();
                var service = conn.GetCrmService();
                var ent = service.Retrieve("contact", id, new ColumnSet("contactid", "firstname", "lastname", "emailaddress1", "mobilephone"));
                
                if (ent != null)
                {
                    contact = new Contact()
                    {
                        Id = new Guid(ent["contactid"].ToString()),
                        emailaddress1 = ent["emailaddress1"].ToString(),
                        mobilephone = ent["mobilephone"].ToString(),
                        Firstname = ent["firstname"].ToString(),
                        Lastname = ent["lastname"].ToString()
                    };
                    result = new
                    {
                        data = contact,
                        message = "Contact Found",
                        statusCode = 200,
                        succeed = true
                    };
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                result = new
                {
                    data = contact,
                    message=ex.Message,
                    statusCode = 500,
                    succeed = false
                };
            }
            

            return Ok(result); // برگرداندن شیء نهایی با data و statusCode
        }
        
        [System.Web.Http.HttpGet]
        [Route("GetContactByMobile")]
        public IHttpActionResult GetContactByMobile(string mobile)
        {
            var conn = new Connection();
            var service = conn.GetCrmService();
            var query = new QueryExpression()
            {
                ColumnSet= new ColumnSet("contactid", "firstname", "lastname", "emailaddress1", "mobilephone"),
                EntityName ="contact",
                Criteria =
                {
                    Filters=
                    {
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("mobilephone",ConditionOperator.Equal,mobile)
                            }
                        }
                    }
                }
            };

            Contact contact = null;
            object result = null;
            try
            {
                var ent = service.RetrieveMultiple(query).Entities.FirstOrDefault();
                if (ent != null)
                {
                    contact = new Contact
                    {
                        Id = new Guid(ent["contactid"].ToString()),
                        emailaddress1 = ent["emailaddress1"].ToString(),
                        mobilephone = ent["mobilephone"].ToString(),
                        Firstname = ent["firstname"].ToString(),
                        Lastname = ent["lastname"].ToString()
                    };
                    result = new
                    {
                        data = contact,
                        message = "Contact Found",
                        statusCode = 200,
                        succeed = true
                    };
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                result = new
                {
                    data = contact,
                    message = ex.Message,
                    statusCode = 500,
                    succeed = false
                };
            }
            return Ok(result); // برگرداندن شیء نهایی با data و statusCode
        }

        [System.Web.Http.HttpGet]
        [Route("GetContactByName")]
        public IHttpActionResult GetContactByName([FromBody] InputParametersForGetContact inp)
        {
            if (!ValidateHeaders(Request))
            {
                return (IHttpActionResult)Unauthorized(); // خطای 401 برمی‌گرداند
            }
            //
            if (inp == null)
            {
                return BadRequest("input parameters is empty or null.");
            }
            var conn = new Connection();
            var service = conn.GetCrmService();
            var query = new QueryExpression();
            query.ColumnSet = new ColumnSet("contactid", "firstname", "lastname", "emailaddress1", "mobilephone");
            query.EntityName = "contact";
            ConditionExpression condition1 = new ConditionExpression("firstname", ConditionOperator.Equal, inp.firstName);
            ConditionExpression condition2 = new ConditionExpression("lastname", ConditionOperator.Equal, inp.lastName);
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            //By this way you can search with firstname or lastname or both of them
            if (!inp.firstName.IsNullOrWhiteSpace())
                filter.AddCondition(condition1);
            if (!inp.lastName.IsNullOrWhiteSpace())
                filter.AddCondition(condition2);
            
            query.Criteria.AddFilter(filter);

            List<Contact> contacts = new List<Contact>();
            object result = null;
            try
            {
                var ent = service.RetrieveMultiple(query).Entities;
                foreach (var item in ent)
                {
                    var contact = new Contact
                    {
                        Id = new Guid(item["contactid"].ToString()),
                        emailaddress1 = item.GetAttributeValue<string>("emailaddress1"),
                        mobilephone = item.GetAttributeValue<string>("mobilephone"),
                        Firstname = item.GetAttributeValue<string>("firstname"),
                        Lastname = item.GetAttributeValue<string>("lastname"),
                    };
                    contacts.Add(contact);
                }
                
                result = new
                {
                    data = contacts,
                    message = "Contact Found",
                    statusCode = 200,
                    succeed = true
                };
            }
            catch (Exception ex)
            {
                result = new
                {
                    data = contacts,
                    message = ex.Message,
                    statusCode = 500,
                    succeed = false
                };
            }
            return Ok(result); // برگرداندن شیء نهایی با data و statusCode
        }
        // POST api/values
        [System.Web.Http.HttpPost]
        [Route("RegisterContact")]
        public IHttpActionResult RegisterContact([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return BadRequest("Customer list is empty or null.");
            }

            try
            {
                var conn = new Connection();
                var service = conn.GetCrmService();
                var ent = new Entity("contact");
                ent["firstname"] = contact.Firstname;
                ent["lastname"] = contact.Lastname;
                ent["emailaddress1"] = contact.emailaddress1;
                ent["mobilephone"] = contact.mobilephone;
                contact.Id = service.Create(ent);
                return Ok(new
                {
                    data = contact,
                    message = "Contact created successfully",
                    statusCode = 200,
                    succeed = true
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    data = contact,
                    message = ex.Message,
                    statusCode = 500,
                    succeed = false
                });
            }
        }

        [System.Web.Http.HttpPost]
        [Route("RegisterMultiContacts")]
        public IHttpActionResult RegisterMultiContacts([FromBody] List<Contact> contacts)
        {
            if (contacts == null || contacts.Count==0)
            {
                return BadRequest("Contact list is empty or null.");
            }
            
            var conn = new Connection();
            var service = conn.GetCrmService();
            var ent = new Entity("contact");
            List<Contact> createdContacts = new List<Contact>();
            List<Guid> createdGUID = new List<Guid>();
            int i = 1;
            try
            {
                foreach (var contact in contacts)
                {
                    ent["firstname"] = contact.Firstname;
                    ent["lastname"] = contact.Lastname;
                    ent["emailaddress1"] = contact.emailaddress1;
                    ent["mobilephone"] = contact.mobilephone;
                    contact.Id = service.Create(ent);
                    createdContacts.Add(contact);
                    createdGUID.Add(contact.Id);
                    i++;
                }
                
                return Ok(new
                {
                    data = createdGUID,
                    message = $"{i-1} Contacts created successfully",
                    statusCode = 200,
                    succeed = true
                });
            }
            catch (Exception ex)
            {
                if (createdGUID.Count > 0)
                    return Ok(new
                    {
                        data = createdGUID,
                        message = $"{i-1} Contacts are created , but {ex.Message}",
                        statusCode = 200,
                        succeed =false
                    });
                else
                    return Ok(new
                    {
                        data = createdGUID,
                        message = $"Creation of Contacts failed",
                        statusCode = 500,
                        succeed = false
                    });
            }
        }



        [System.Web.Http.HttpPut]
        [Route("UpdateContactByMobile")]
        public IHttpActionResult UpdateContactByMobile(string mobile, [FromBody] Contact contact)
        {
            var conn = new Connection();
            var service = conn.GetCrmService();
            var query = new QueryExpression()
            {
                ColumnSet = new ColumnSet("contactid", "firstname", "lastname", "emailaddress1", "mobilephone"),
                EntityName = "contact",
                Criteria =
                {
                    Filters=
                    {
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("mobilephone",ConditionOperator.Equal,mobile)
                            }
                        }
                    }
                }
            };


            try
            {
                var ent = service.RetrieveMultiple(query).Entities.FirstOrDefault();
                if (ent != null)
                {
                    if (!contact.Firstname.IsNullOrWhiteSpace())
                        ent["firstname"] = contact.Firstname;
                    if (!contact.Lastname.IsNullOrWhiteSpace())
                        ent["lastname"] = contact.Lastname;
                    if (!contact.mobilephone.IsNullOrWhiteSpace())
                        ent["mobilephone"] = contact.mobilephone;
                    if (!contact.mobilephone.IsNullOrWhiteSpace())
                        ent["emailaddress1"] = contact.emailaddress1;
                    service.Update(ent);

                    return Ok(new
                    {
                        data = contact,
                        message = "Update Successfuly",
                        statusCode = 200,
                        succeed = true
                    });
                }
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                        data = new object(),
                        message = ex.Message,
                        statusCode = 500,
                        succeed = false
                });
            }
        }

        public void Delete(int id)
        {
        }
    }
}
