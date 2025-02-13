using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class ThoughtController : Controller
    {
        // GET: Thought
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ThoughtIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Though_List";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }
            int Orgid = Convert.ToInt32(userSession.id_ORGANIZATION);


            ThoughtsModel thoughtModel = new ThoughtsModel();

            List<Thoughts> thoughtModelList = thoughtModel.ThoughtList(Orgid);

            ViewData["thoughtModelList"] = thoughtModelList;

            return View();
        }

        [HttpPost]
        public ActionResult ThoughtSaveConfiguration(Thoughts model)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Thought";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";

                UserLogDetails userLogDetails = new UserLogDetails();
                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
            }
            string imageFileName = null;
         


                 string textonly = Request.Form["onlytextInput"];
                    if(textonly != "")
                    {
                       model.ThoughtsName = textonly;
                    }
                // Handle file uploads
                HttpPostedFileBase imageFile = Request.Files["Image_path"];
                HttpPostedFileBase videoFile = Request.Files["webVideofile-input"];
                HttpPostedFileBase gifFile = Request.Files["GifFileInput"];

                if (imageFile != null && imageFile.ContentLength > 0)
                {


                    string uploadDir = "~/Content/SKILLMUNI_DATA/Thoughts";
                    string serverPath = Server.MapPath(uploadDir);

                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    imageFileName = Path.GetFileName(imageFile.FileName);
                    string imagePath = Path.Combine(serverPath, imageFileName);
                    imageFile.SaveAs(imagePath);
                    model.ThoughtsName = imageFileName;

                }

                if (videoFile != null && videoFile.ContentLength > 0)
                {
                    // Define the directory for saving files
                    string uploadDir = "~/Content/SKILLMUNI_DATA/Thoughts";
                    string serverPath = Server.MapPath(uploadDir);

                    // Ensure the directory exists
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    // Use the original file name
                    string originalFileName = Path.GetFileName(videoFile.FileName);

                    // Combine the server path with the original file name
                    string videoPath = Path.Combine(serverPath, originalFileName);



                    // Save the GIF file to the server
                    videoFile.SaveAs(videoPath);

                    // Save the file name to the model
                    model.ThoughtsName = originalFileName;
                }

                if (gifFile != null && gifFile.ContentLength > 0)
                {
                    // Define the directory for saving files
                    string uploadDir = "~/Content/SKILLMUNI_DATA/Thoughts";
                    string serverPath = Server.MapPath(uploadDir);

                    // Ensure the directory exists
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    // Use the original file name
                    string originalFileName = Path.GetFileName(gifFile.FileName);

                    // Combine the server path with the original file name
                    string gifFilePath = Path.Combine(serverPath, originalFileName);



                    // Save the GIF file to the server
                    gifFile.SaveAs(gifFilePath);

                    // Save the file name to the model
                    model.ThoughtsName = originalFileName;
                }
            



            Thoughts temp = new Thoughts
                {
                    IdThoughts = model.IdThoughts,
                    IdOrganization = Convert.ToInt32(userSession.id_ORGANIZATION),
                    IdCmsUser = Convert.ToInt32(userSession.ID_USER),
                    ThoughtsName = model.ThoughtsName,
                    StartDateTime = model.StartDateTime,
                    ExpiredDate = model.ExpiredDate
                };

                bool CategoryAddedSuccessfully = new ThoughtsModel().AddThoughts(temp).Equals("TRUE");

                if (CategoryAddedSuccessfully)
                {
                    TempData["MessageVideo"] = "Data Inserted Successfully";
                    return RedirectToAction("ThoughtIndex");
                }
                else
                {
                    TempData["MessageVideo"] = "Data Insertion Failed";
                    return RedirectToAction("ThoughtIndex");
                }
            


          
        }

        [HttpPost]
        public ActionResult ThoughtDeleteConfiguration(int num)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Thought";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Delete";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            bool categoryDeletedSuccessfully = new ThoughtsModel().DeleteThough(num);

            if (categoryDeletedSuccessfully)
            {
                // If successful, return JSON with success message
                return Json(new { success = true });
            }
            else
            {
                // If failure, redirect to the "CategoryIndex" action
                return RedirectToAction("CategoryIndex");
            }


        }

    }
}