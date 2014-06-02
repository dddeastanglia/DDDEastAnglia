using System.Web.Mvc;
using DDDEastAnglia.Filters;

namespace DDDEastAnglia
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            FilterProviders.Providers.Add(new PreviewFilterProvider());
        }
    }
}
