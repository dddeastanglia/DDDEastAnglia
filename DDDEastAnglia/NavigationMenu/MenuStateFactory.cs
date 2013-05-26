using System;
using System.Web.Routing;

namespace DDDEastAnglia.NavigationMenu
{
    public interface IMenuStateFactory
    {
        MenuState Create(RouteData routeData);
    }

    public class MenuStateFactory : IMenuStateFactory
    {
        public MenuState Create(RouteData routeData)
        {
            if (routeData == null)
            {
                throw new ArgumentNullException("routeData");
            }
            
            return new MenuState(routeData);
        }
    }
}
