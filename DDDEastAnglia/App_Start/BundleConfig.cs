using System.Web.Optimization;

namespace DDDEastAnglia
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            AddScriptBundle(bundles, "~/bundles/jquery",
                            "~/Scripts/jquery-{version}.js",
                            "~/Scripts/jquery.cookie.js");

            AddScriptBundle(bundles, "~/bundles/jqueryui",
                            "~/Scripts/jquery-ui-{version}.js");

            AddScriptBundle(bundles, "~/bundles/jqueryval",
                            "~/Scripts/jquery.unobtrusive*",
                            "~/Scripts/jquery.validate*");

            AddScriptBundle(bundles, "~/bundles/tablesorter",
                            "~/Scripts/jquery.tablesorter*");

            AddScriptBundle(bundles, "~/bundles/searchfilter",
                            "~/Scripts/searchfilter.js");

            AddScriptBundle(bundles, "~/bundles/bootstrap",
                            "~/Scripts/bootstrap.js");

            AddScriptBundle(bundles, "~/bundles/Markdown",
                            "~/Scripts/Markdown*");

            AddScriptBundle(bundles, "~/bundles/flot",
                            "~/Scripts/flot/jquery.flot.js",
                            "~/Scripts/flot/jquery.flot.time.js",
                            "~/Scripts/flot/jquery.flot.axislabel.js");

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            AddScriptBundle(bundles, "~/bundles/modernizr",
                            "~/Scripts/modernizr-*");

            AddScriptBundle(bundles, "~/bundles/voting", "~/Scripts/voting.js");
            AddScriptBundle(bundles, "~/bundles/resolveIPAddress", "~/Scripts/resolveIPAddress.js");
            AddScriptBundle(bundles, "~/bundles/progressbar", "~/Scripts/progressbar.js");

            AddStyleBundle(bundles, "~/Content/common",
                            "~/Content/bootstrap.css",
                            "~/Content/media-queries.css",
                            "~/Content/font-awesome.css",
                            "~/Content/font-awesome-ie7.min.css",
                            "~/Content/common.css");

            AddStyleBundle(bundles, "~/Content/admin",
                           "~/Areas/Admin/Content/admin.css");

            AddStyleBundle(bundles, "~/Content/css",
                           "~/Content/Site.css");

            AddStyleBundle(bundles, "~/Content/themes/base/css",
                           "~/Content/themes/base/jquery.ui.core.css",
                           "~/Content/themes/base/jquery.ui.resizable.css",
                           "~/Content/themes/base/jquery.ui.selectable.css",
                           "~/Content/themes/base/jquery.ui.accordion.css",
                           "~/Content/themes/base/jquery.ui.autocomplete.css",
                           "~/Content/themes/base/jquery.ui.button.css",
                           "~/Content/themes/base/jquery.ui.dialog.css",
                           "~/Content/themes/base/jquery.ui.slider.css",
                           "~/Content/themes/base/jquery.ui.tabs.css",
                           "~/Content/themes/base/jquery.ui.datepicker.css",
                           "~/Content/themes/base/jquery.ui.progressbar.css",
                           "~/Content/themes/base/jquery.ui.theme.css");

            AddStyleBundle(bundles, "~/Content/Markdown",
                           "~/Content/Markdown.css");

            AddStyleBundle(bundles, "~/Content/Site_print", "~/Content/Site_print.css");
        }

        private static void AddScriptBundle(BundleCollection bundles, string virtualPath, params string[] scriptFiles)
        {
            bundles.Add(new ScriptBundle(virtualPath).Include(scriptFiles));
        }

        private static void AddStyleBundle(BundleCollection bundles, string virtualPath, params string[] styleFiles)
        {
            bundles.Add(new StyleBundle(virtualPath).Include(styleFiles));
        }
    }
}