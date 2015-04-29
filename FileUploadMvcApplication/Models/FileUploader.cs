using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FileUploadMvcApplication.Models
{
    public class FileUploader
    {
        
        public string LinkText { get; set; }

        public Guid Id { get; set; }

        [ValidateFile]
        public string FileName { get; set; }
        
        //public byte[] File { get; set; }
        
        public bool Approved { get; set; }

        public IList<HttpPostedFileBase> UploadFile { get; set; }

    }
}