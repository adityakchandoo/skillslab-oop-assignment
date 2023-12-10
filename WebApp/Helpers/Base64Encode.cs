using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Helpers
{
    public static class Base64Encode
    {

        public static string StringToBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            byte[] bytesToEncode = Encoding.UTF8.GetBytes(input);
            string encodedText = Convert.ToBase64String(bytesToEncode);

            return encodedText;
        }
    }
}