using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using FileUploadMvcApplication.Models;

namespace FileUploadMvcApplication.Controllers
{
    public class FileUploadController : Controller
    {
        public ActionResult UploadDocument(FileUploader viewModel)
        {
            viewModel = (FileUploader) Session["viewModel"];
            viewModel.FileName = viewModel.UploadFile.FileName;

            UpdateXmlFile(viewModel);

            //TODO: move this to the button
            GetSiteHTML();

            return View(viewModel);
        }

        private static void GetSiteHTML()
        {
            string urlAddress = "http://google.com";

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            using (
                FileStream fs = new FileStream(Path.Combine(HttpRuntime.AppDomainAppPath, "test.htm"), FileMode.Create))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(data);
                    }

                    //TODO: Create the search and replace text: Search - Search the world, Replace - Rule the world now
                    //This is a mock up

                    //FileInfo fi = new FileInfo(Path.Combine(HttpRuntime.AppDomainAppPath, "test.htm"));
                    //var ZipCodesAndCountryCodes = File.ReadLines(fi.FullName).Select(l =>
                    //{
                    //    var countrySubstr = l.Substring(1405, 30);
                    //    return new
                    //    {
                    //        ZipCode = l.Substring(1395, 5),
                    //        CountryCode = string.IsNullOrWhiteSpace(countrySubstr)
                    //                      || countrySubstr == "USA"
                    //                      || countrySubstr == "United States"
                    //                      || countrySubstr == "United States of America"
                    //            ? "US"
                    //            : countrySubstr
                    //    };
                    //});



                    response.Close();
                    readStream.Close();
                }
            }
        }

        private void UpdateXmlFile(FileUploader viewModel)
        {
            XElement xEle = XElement.Load(Path.Combine(HttpRuntime.AppDomainAppPath, "Files.xml"));
            xEle.Add(new XElement("FileName", viewModel.FileName));
            xEle.Save(Path.Combine(HttpRuntime.AppDomainAppPath, "Files.xml"));
        }

        // POST: /FileUpload/
        [HttpPost]
        public ActionResult ProcessUploadedFile(FileUploader viewModel, HttpPostedFileBase uploadFile)
        {
            //TODO: check for null file, check for file extensions, then keep one page

            var fileName = Path.GetFileName(uploadFile.FileName);
            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            uploadFile.SaveAs(path);
            
            Session["viewModel"] = viewModel;
            return RedirectToAction("UploadDocument");
        }

    }
}
