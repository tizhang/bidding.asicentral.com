using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;
using Bidding.Web.Helpers;

namespace Bidding.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                    .IncludeDirectory("~/App", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ng-table.css",
                      "~/App/vendor/node_modules/angularjs-datetime-picker/angularjs-datetime-picker.css",
                      "~/Content/site.css"));

            var angModules = new Dictionary<string, string>() {
                {"bidding", "App/modules/bidding/partials"},
                {"settings", "App/modules/settings/partials"}
            };

            var partials = new AngularPartialsBundle(angModules, "~/bundles/partials")
                .IncludeDirectory("~/App/modules", "*.html", true);
            bundles.Add(partials);

        }
    }
}
