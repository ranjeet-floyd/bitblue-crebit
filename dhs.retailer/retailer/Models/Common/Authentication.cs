using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace com.dhs.webapi.Model.Common
{
    public class Authentication : IPrincipal
    {
        public String UserName { get; set; }
        public IIdentity Identity { get; set; }
        public bool IsInRole(string role)
        {
            if (role.Equals("User"))
                return true;
            else
                return false;
        }

        //Constructor
        public Authentication(string userName)
        {
            this.UserName = userName;
            this.Identity = new GenericIdentity(userName);
        }

    }
}