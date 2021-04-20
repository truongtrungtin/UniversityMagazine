using EntityModels.EF;
using System.Linq;
using System.Web.Mvc;

namespace UniversityMagazine.Areas.Management.Controllers
{
    public class TermAndConditionController : BaseController
    {
        // GET: Management/TermAndCondition
        [HttpGet]
        public ActionResult Index()
        {
            UniversityMagazineDBContext db = new UniversityMagazineDBContext();
            var model = db.TERMCONDITIONs.FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(TERMCONDITION tERMCONDITION)
        {
            UniversityMagazineDBContext db = new UniversityMagazineDBContext();
            var model = db.TERMCONDITIONs.FirstOrDefault();
            model.TERMCONDITION_Content = tERMCONDITION.TERMCONDITION_Content;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}