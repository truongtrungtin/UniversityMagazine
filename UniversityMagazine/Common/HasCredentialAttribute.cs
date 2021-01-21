﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace UniversityMagazine.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string ROLE_Code { set; get; }
        public string ROLEGROUP_Name { set; get; }
        public bool CREDENTIAL_VIEW { set; get; }
        public bool CREDENTIAL_ADD { set; get; }
        public bool CREDENTIAL_EDIT { set; get; }
        public bool CREDENTIAL_DELETE { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserLogin)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                return false;
            }

            IEnumerable<Credential> privilegeLevels = this.GetCredentialByLoggedInUser(); // Call another method to get rights of the user from DB
            foreach (var item in privilegeLevels)
            {
                if (item.ROLEGROUP_Name == "ADMIN")
                {
                    return true;
                }
                else if (item.ROLEGROUP_Name == ROLEGROUP_Name)
                {
                    if (item.CREDENTIAL_VIEW == CREDENTIAL_VIEW && item.CREDENTIAL_VIEW == true)
                    {
                        return true;
                    }
                    else if (item.CREDENTIAL_ADD == CREDENTIAL_ADD && item.CREDENTIAL_ADD == true)
                    {
                        return true;
                    }
                    else if (item.CREDENTIAL_EDIT == CREDENTIAL_EDIT && item.CREDENTIAL_EDIT == true)
                    {
                        return true;
                    }
                    else if (item.CREDENTIAL_DELETE == CREDENTIAL_DELETE && item.CREDENTIAL_DELETE == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    continue;
                }
            }
            return false;

        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = (UserLogin)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Error/Error401.cshtml"
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Login/Index.cshtml"
                };
            }


        }
        private IEnumerable<Credential> GetCredentialByLoggedInUser()
        {
            var credentials = (IEnumerable<Credential>)HttpContext.Current.Session[CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}