using System.Web;
using System.Web.Optimization;

namespace VGExplorerWeb
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Default/css").Include(
                                "~/Content/themes/Default/bootstrap*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                              "~/Content/site.css"));
        }
    }
}