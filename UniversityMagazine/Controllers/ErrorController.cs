using System.Web.Mvc;

namespace UniversityMagazine.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error403()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }
    }
}