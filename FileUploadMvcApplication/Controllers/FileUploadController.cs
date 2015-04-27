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
using HtmlAgilityPack;

namespace FileUploadMvcApplication.Controllers
{
    public class FileUploadController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        // POST: /FileUpload/
        [HttpPost]
        public ActionResult ProcessUploadedFile(FileUploader viewModel, HttpPostedFileBase uploadFile)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }
            else
            {
                //Get file and save it
                var fileName = Path.GetFileName(uploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                uploadFile.SaveAs(path);

                //Session["viewModel"] = fileName;
                //viewModel = Session["viewModel"] as FileUploader;
                //viewModel.FileName = viewModel.UploadFile.FileName;

                //TODO: Xml file does not have file name
                //Call function to update the xml file
                UpdateXmlFile(viewModel);


                //GetSiteHTML();

              

                //TODO: view does not have file name
                return View("UploadDocument", viewModel);
            }
        }

        
        private void UpdateXmlFile(FileUploader viewModel)
        {
            XElement xEle = XElement.Load(Path.Combine(HttpRuntime.AppDomainAppPath, "Files.xml"));
            xEle.Add(new XElement("FileName", viewModel.FileName));
            xEle.Save(Path.Combine(HttpRuntime.AppDomainAppPath, "Files.xml"));
        }

        private void GetSiteHTML()
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
                    HtmlDocument testDocument = new HtmlDocument();
                    testDocument.LoadHtml(Server.MapPath("~/test.htm"));

                    var textNodes = testDocument.DocumentNode.SelectNodes("/div/text()[contains(.,'Search the world')]");
                    if (textNodes != null)
                        foreach (HtmlTextNode node in textNodes)
                            node.Text = node.Text.Replace("Search the world", "Rule the world now");

                    //testDocument.Save();

                }

                //public ActionResult UploadDocument(FileUploader viewModel)
                //{
                //    if (!ModelState.IsValid)
                //    {
                //        return View(viewModel);
                //    }

                //    viewModel = (FileUploader)Session["viewModel"];
                //    viewModel.FileName = viewModel.UploadFile.FileName;

                //    UpdateXmlFile(viewModel);

                //    //TODO: move this to the button
                //    GetSiteHTML();

                //    return Content("Thanks for uploading", "text/plain");
                //}

            }
        }
    }
}
