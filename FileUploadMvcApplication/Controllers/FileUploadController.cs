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

                Session["viewModel"] = fileName;
                viewModel.FileName = viewModel.UploadFile.FileName;

                //Call function to update the xml file
                //TODO: uncomment
                //UpdateXmlFile(viewModel);

                //TODO: uncomment
                GetSiteHTML();

                ReplaceString();


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

                    //TODO: uncomment and remove from ProcessUpload method
                    //ReplaceString();
                }

            }
        }

        private void ReplaceString()
        {
            //TODO: Create the search and replace text: Search - Search the world, Replace - Rule the world now
            //HtmlDocument testDocument = new HtmlDocument();
            //testDocument.LoadHtml(Server.MapPath("~/test.htm"));

            //var textNodes = testDocument.DocumentNode.SelectNodes("/div/text()[contains(.,'Search the world')]");
            //if (textNodes != null)
            //    foreach (HtmlTextNode node in textNodes)
            //        node.Text = node.Text.Replace("Search the world", "Rule the world");

            //testDocument.Save();

            StreamReader reading = System.IO.File.OpenText(Server.MapPath("~/test.htm"));
            string str;
            while ((str = reading.ReadLine()) != null)
            {
                if (str.Contains("some text"))
                {
                    File.WriteAllText("test.txt", File.ReadAllText("test.txt").Replace("some text", "some other text"));
                    StreamWriter write = new StreamWriter("test.txt");
                }
            }
        }
    }
}
