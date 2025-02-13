using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class DepartmentController : Controller
    {
      
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DepartmentIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Department List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);

            }
            int Orgid = Convert.ToInt32(userSession.id_ORGANIZATION);
            AddDepaetment_Model departmentModel = new AddDepaetment_Model();


       
           
           
           
            List<tbl_department> departmentList = departmentModel.GetDepartmentData(Orgid);
            ViewData["departmentList"] = departmentList;
            return View();
        }

        public ActionResult DepartmenttSaveConfiguration(FormCollection formCollection)
        {
            tbl_department temp = new tbl_department();
            if (int.TryParse(this.Request.Form["Id_department"], out int departmentId))
            {
                temp.Id_department = departmentId;
            }
            else
            {
                temp.Id_department = 0;
            }
            string str4 = this.Request.Form["Department_name"].Trim();
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
           
            try
            {
               
              
                temp.Department_name = str4;
           
                temp.Status = "A";
         
                temp.Id_org = Convert.ToInt32(content.id_ORGANIZATION);
            }
            catch (Exception ex)
            {
                new contentDashboardModel().exception_log(ex);
            }
            return new AddDepaetment_Model().AddDepaetment(temp).Equals("TRUE") ? (ActionResult)this.RedirectToAction("DepartmentIndex") : (ActionResult)this.RedirectToAction("DepartmentIndex");
        }


        [HttpPost]
        public ActionResult DeleteDepartment(int num)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Department";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Delete";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            bool categoryDeletedSuccessfully = new AddDepaetment_Model().DeleteDepartment(num);

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