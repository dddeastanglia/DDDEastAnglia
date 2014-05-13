using System.Web.Mvc;

namespace DDDEastAnglia.Tests
{
    public static class ActionResultExtensions
    {
        public static T GetViewModel<T>(this ActionResult actionResult)
        {
            var result = (ViewResult) actionResult;
            var model = (T) result.Model;
            return model;
        }
    }
}
