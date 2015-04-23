using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileUploadMvcApplication.Models
{
    public class FileUploader
    {
        
        public string LinkText { get; set; }
        public string FileName { get; set; }
        public Guid Id { get; set; }
        public bool Approved { get; set; }
        
        //[ValidateFile(ErrorMessage = "false!")]
        public HttpPostedFileBase UploadFile { get; set; }

    }
}