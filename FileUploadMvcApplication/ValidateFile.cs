using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace FileUploadMvcApplication
{
    public class ValidateFile : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            List<string> allowedExtensions = new List<string>() {".pdf", ".xml", ".xsd"};

            //Todo: This goes through 2x. The second time is null - after UpdateXmlFile method
            //Testing with .pdfi - returns false, but is going through 2x and has null 2x
            //var result1 = allowedExtensions.Any(v => value.ToString().EndsWith(v));

            if ((file == null) || (file.ContentLength > 1*1024*1024) ||
                (!allowedExtensions.Any(v => value.ToString().EndsWith(v))))
            {
                return false;
            }

            return true;

        }
    }
}