using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using FileUploadMvcApplication.Models;

namespace FileUploadMvcApplication.Controllers
{
    public class FileUploadController : Controller
    {
        public ActionResult UploadDocument(FileUploader viewModel)
        {
            viewModel = (FileUploader)Session["viewModel"];
            viewModel.FileName = viewModel.UploadFile.FileName;

            //FileUploader newFile = new FileUploader();
            //newFile.FileName = viewModel.FileName;
            //System.Xml.Serialization.XmlSerializer writer =
            //    new System.Xml.Serialization.XmlSerializer(typeof(FileUploader));

            //System.IO.StreamWriter file = new System.IO.StreamWriter(
            //    );
            //writer.Serialize(file, newFile);
            //file.Close();

            //TODO: refactor into UpdateXMLFile
            XmlDocument xd = new XmlDocument();
            xd.Load(Server.MapPath("~/Files.xml"));
            XmlNode nl = xd.SelectSingleNode("//Files");
            XmlDocument xd2 = new XmlDocument();
            xd2.LoadXml("<FileUploader><FileName>" + viewModel.FileName + "</FileName></FileUploader>");
            XmlNode n = xd.ImportNode(xd2.FirstChild, true);
            nl.AppendChild(n);
            xd.Save(Server.MapPath("~/Files.xml"));

            //TODO: refactor into GetSiteHTML
            string urlAddress = "http://google.com";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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
                Console.WriteLine(data);

                response.Close();
                readStream.Close();
            }

            return View(viewModel);
            }

                // POST: /FileUpload/
            [
            HttpPost]
        public ActionResult ProcessUploadedFile(FileUploader viewModel, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadFile == null)
                { ModelState.AddModelError("File", "Please upload your file"); }
            }
            else if (uploadFile.ContentLength > 0)
            {
                //int MaxContentLength = 1024 * 1024 * 3; 
                string[] allowedExtensions = new string[] { ".pdf", ".xml", ".xsd" };

                if (!allowedExtensions.Contains(uploadFile.FileName.Substring(uploadFile.FileName.LastIndexOf('.'))))
                {
                    ModelState.AddModelError("File", "Please choose an apporved file type");
                }
                else
                {
                    var fileName = Path.GetFileName(uploadFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    uploadFile.SaveAs(path);
                }
            }
            Session["viewModel"] = viewModel;
            return RedirectToAction("UploadDocument");
        }
        
    }
}
