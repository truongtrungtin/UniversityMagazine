using System.Web.Mvc;
using UniversityMagazine.DAO;

namespace UniversityMagazine.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}