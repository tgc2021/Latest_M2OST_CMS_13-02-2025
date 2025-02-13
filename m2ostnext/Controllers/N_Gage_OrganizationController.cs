using m2ostnext.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static m2ostnext.Models.Ngage;
using static Mysqlx.Notice.Warning.Types;

namespace m2ostnext.Controllers
{
    public class N_Gage_OrganizationController : Controller
    {
        // GET: N_Gage_Organization
        public ActionResult Index()
        {
            return View();
        }
        public class tbl_org_mapping
        {
            public int Id_orgmap { get; set; }
            public string M2ost { get; set; }
            public int Ngage { get; set; }


        }
        public ActionResult N_GageIndex()
        {
            int orgId = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

            Ngage_service kpiService1 = new Ngage_service();
            List<SelectListItem> OrgIDList = kpiService1.GetOrgIdList();
            ViewBag.OrgIDList = OrgIDList;

            return View();
        }

        [HttpPost]
        public ActionResult SaveConfiguration(int SelectedOrgID)
        {
            try
            {
                int orgId = 0;
                var userSession = this.HttpContext.Session["UserSession"] as UserSession;
                if (userSession != null)
                {
                    orgId = Convert.ToInt32(userSession.id_ORGANIZATION);
                }
                else
                {
                    TempData["ErrorMessage"] = "User session is invalid.";
                    return RedirectToAction("N_GageIndex");
                }

                // Database query
                string query = @"INSERT INTO tbl_org_mapping (M2ost, Ngage) VALUES (@M2ost, @Ngage)";

                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["db_tgc_gameEntities1"].ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@M2ost", orgId);
                        command.Parameters.AddWithValue("@Ngage", SelectedOrgID);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Organizational saved successfully.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to save configuration.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Replace this with your logging framework
                Console.WriteLine($"Error: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while saving the configuration.";
            }

            return RedirectToAction("N_GageIndex");
        }

    }
}