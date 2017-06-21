using System;
using System.Web.Routing;

namespace DDDEastAnglia.NavigationMenu
{
    public class MenuState
    {
        private readonly RouteData routeData;

        public MenuState(RouteData routeData)
        {
            if (routeData == null)
            {
                throw new ArgumentNullException(nameof(routeData));
            }

            this.routeData = routeData;
        }

        public bool IsCurrentlySelectedItem(string controllerName, string actionName)
        {
            var currentController = routeData.GetRequiredString("controller");
            var currentAction = routeData.GetRequiredString("action");
            return actionName == currentAction && controllerName == currentController;
        }
    }
}
