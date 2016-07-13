using System.Web.Mvc;

namespace WebApplication1.Areas.Blabla
{
    public class BlablaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Blabla";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Blabla_default",
                "Blabla/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}