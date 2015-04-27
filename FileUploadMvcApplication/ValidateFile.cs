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

            if (!allowedExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload your file of type: " + string.Join(", ", allowedExtensions);
                return false;
            }

            return true;
        }
    }
}