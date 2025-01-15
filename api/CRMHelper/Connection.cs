using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace api.CRMHelper
{
    public class Connection
    {
        public  IOrganizationService GetCrmService()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var crmCNS = "Server=http://10.104.50.57/mobinnet; Domain=mobinnet; Username=PublicAPI; Password=%CrM*524;";

                CrmServiceClient service = new CrmServiceClient(crmCNS.ToString());

                return (IOrganizationService)service.OrganizationServiceProxy;
            }
            catch (Exception)
            {
                throw null;
            }
        }
    }
}