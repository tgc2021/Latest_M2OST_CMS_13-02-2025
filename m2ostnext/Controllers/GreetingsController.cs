using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class GreetingsController : Controller
    {
        // GET: Greetings
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GreetingsIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Greetings_List";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }
            int Orgid = Convert.ToInt32(userSession.id_ORGANIZATION);


            ThoughtsModel thoughtModel = new ThoughtsModel();

            List<Greeting> GreetingModelList = thoughtModel.GreetingList(Orgid);

            ViewData["GreetingModelList"] = GreetingModelList;

            return View();
        }
    }
}