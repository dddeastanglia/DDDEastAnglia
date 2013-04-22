using System;
using System.Linq.Expressions;
using System.Web.Mvc;


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

    public static string MenuState(this HtmlHelper htmlHelper, string controllerName)
    {
        var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
        if (controllerName == currentController)
        {
            return "active";
        }
        return "";
    }

    public static string MenuState(this HtmlHelper htmlHelper, string controllerName, string actionName)
    {
        var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
        var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
        if (actionName == currentAction && controllerName == currentController)
        {
            return "active";
        }
        return "";
    }

    public static MvcHtmlString TweetButton(this HtmlHelper htmlHelper, string tweetText, Uri url = null)
    {
        var encodedTweetText = Uri.EscapeDataString(tweetText); // escape anything URI-unfriendly in the tweet body (e.g., hash-tags)
        
        if (url != null)
        {
            encodedTweetText += " " + Uri.EscapeUriString(url.ToString()); // escape the URL
        }

        encodedTweetText = htmlHelper.Encode(encodedTweetText); // HTML-encode the resulting string
        return new MvcHtmlString(
            string.Format(
                @"<a href=""https://twitter.com/intent/tweet?text={0}"" class=""btn btn-mini"" target=""_blank""><i class=""icon-twitter""></i> Tweet</a>",
                encodedTweetText));
    }
}
