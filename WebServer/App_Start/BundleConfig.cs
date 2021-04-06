using System.Web;
using System.Web.Optimization;

namespace WebServer {
    public class BundleConfig {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/GameStyle").Include(
                      "~/Content/GameStyle.css"));

            bundles.Add(new StyleBundle("~/Content/LobbyStyle").Include(
                      "~/Content/Lobby.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.signalR").Include(
                        "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/game").Include(
                        "~/Scripts/game.js"));

            bundles.Add(new ScriptBundle("~/bundles/javascriptTest").Include(
                        "~/Scripts/javascriptTest.js"));

            bundles.Add(new ScriptBundle("~/bundles/purify").Include(
                        "~/lib/dompurify/purify.js"));
        }
    }
}
