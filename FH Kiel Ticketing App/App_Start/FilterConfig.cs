using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
