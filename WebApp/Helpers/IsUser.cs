using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Helpers
{
    public static class SessionHelpers
    {
        public static bool IsUser(UserRoleEnum role)
        {
            // Check if the session variable exists and if it's equal to the specified value
            if (HttpContext.Current.Session["Role"] != null)
            {
                return (int)HttpContext.Current.Session["Role"] == (int)role;
            }

            return false;
        }
    }
}