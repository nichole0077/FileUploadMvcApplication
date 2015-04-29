using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileUploadMvcApplication
{
    public class ValidateFile : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var fileList = value as IList<HttpPostedFileBase>;
            var allowedExtensions = new List<string> {".pdf", ".xml", ".xsd"};

            if (fileList == null)
            {
                {
                    ErrorMessage = "Please choose a file of type: " + string.Join(", ", allowedExtensions);
                    return false;
                }
            }
            if (fileList.Any(file => !allowedExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')))))
            {
                ErrorMessage = "Your file was not the correst type. Please upload your file of type: " +
                               string.Join(", ", allowedExtensions);
                return false;
            }
            

            return true;
        }
    }
}