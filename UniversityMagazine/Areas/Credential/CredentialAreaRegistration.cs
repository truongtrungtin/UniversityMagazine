using System.Web.Mvc;

namespace UniversityMagazine.Areas.Credential
{
    public class CredentialAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Credential";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Credential_default",
                "Credential/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}