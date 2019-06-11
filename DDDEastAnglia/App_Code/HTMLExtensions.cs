using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DDDEastAnglia.Models;
using MarkdownSharp;

public static class HTMLExtensions
{
    public static MvcHtmlString Markdown(this HtmlHelper<Session> helper, string value)
    {
        Markdown converter = new Markdown();
        return new MvcHtmlString(converter.Transform(value));
    }

    public static MvcHtmlString MarkdownFor<TModel, TValue>(
        this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        where TModel : class
    {
        string rawText = (string)ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
        Markdown converter = new Markdown();
        return new MvcHtmlString(converter.Transform(rawText));
    }

    public static MvcHtmlString DDDEastAnglia(this HtmlHelper helper)
    {
        return new MvcHtmlString(@"<strong class=""dddea"">DDD East Anglia</strong>");
    }

    public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, bool displayCondition)
    {
        return displayCondition ? htmlHelper.ActionLink(linkText, actionName, controllerName) : new MvcHtmlString(linkText);
    }

    public static string IpAddressToId(this HtmlHelper htmlHelper, string ipAddress)
    {
        return ipAddress.Replace(".", "-");
    }
}
