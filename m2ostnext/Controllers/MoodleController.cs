using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class MoodleController : Controller
    {
        // GET: Moodle

        public ActionResult MoodelIndex()
        {  
            string apiUrl = "https://m2ostelearning.com/moodle/auth/userkey/admin_auth.php?userid=admin";

            using (var httpClient = new HttpClient())
            {
                // Call the API and get the response
                var response = httpClient.GetStringAsync(apiUrl).Result;

                // Parse the JSON response
                var jsonResponse = JObject.Parse(response);

                // Extract the "loginurl" from the response
                string loginUrl = jsonResponse["loginurl"]?.ToString();

                if (!string.IsNullOrEmpty(loginUrl))
                {
                    // Return the URL to the client
                    return Json(new { url = loginUrl }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Handle error if loginurl is not present
                    return Json(new { error = "Login URL not found in the response." }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult RedirectToLogin()
        {
           

            return View();  
        }
    }
}