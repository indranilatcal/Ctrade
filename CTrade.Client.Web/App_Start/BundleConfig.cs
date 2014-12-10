using System.Web;
using System.Web.Optimization;

namespace CTrade.Client.Web
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            // administration
            bundles.Add(new ScriptBundle("~/admin/bundles/jquery").Include(
            //"~/Scripts/jquery-1.10.2.min.js",
            //"~/Scripts/jquery-1.11.0.min.js",
            "~/Scripts/jquery-2.1.1.min.js",
            "~/Scripts/jquery.validate.min.js",
            "~/Scripts/jquery.cookie.js",
            "~/Scripts/additional-methods.min.js",
            "~/Scripts/jquery-migrate-1.2.1.min.js",
            "~/Scripts/jquery-ui-1.10.3.custom.min.js",
            "~/Scripts/bootstrap.min.js",
            "~/Scripts/bootstrap-hover-dropdown.min.js",
            "~/Scripts/jquery.slimscroll.min.js",
            "~/Scripts/jquery.blockui.min.js",
            "~/Scripts/jquery.uniform.min.js",
            "~/Scripts/select2.min.js",
            "~/Scripts/jquery.dataTables.min.js",
            "~/Scripts/dataTables.bootstrap.js",
            "~/Scripts/bootstrap-datepicker.js",
            "~/Scripts/toastr.min.js",
            "~/Scripts/ui-toastr.js",
            "~/Scripts/plugins/jquery.multi-select.js",
            "~/Scripts/plugins/app.js",
            "~/Scripts/datatable.js",
            "~/Scripts/wysihtml5-0.3.0.js",
            "~/Scripts/bootstrap-wysihtml5.js",
            "~/Scripts/moment.min.js",
            "~/Scripts/daterangepicker.js"
            //"~/Scripts/plugins/table-editable.js"           
            ));

            bundles.Add(new ScriptBundle("~/admin/bundles/custommodules").IncludeDirectory("~/Scripts/modules", "*.js"));

            bundles.Add(new StyleBundle("~/admin/Content/Styles/base/css").Include(
            "~/Content/Styles/site.css",
            "~/Content/Styles/bootstrap.min.css",
            "~/Content/Styles/spinner.css"
            ));

            bundles.Add(new StyleBundle("~/admin/Content/Styles/font/css").Include(
            "~/Content/Styles/font-awesome/font-awesome.min.css"
            ));

            bundles.Add(new StyleBundle("~/admin/Content/Styles/theme/css").Include(
            //"~/Content/Styles/themes/uniform.default.css",
            "~/Content/Styles/themes/style-conquer.css",
            "~/Content/Styles/themes/style.css",
            "~/Content/Styles/themes/style-responsive.css",
            "~/Content/Styles/themes/default.css",
            "~/Content/Styles/themes/simple-line-icons.min.css"
            ));

            bundles.Add(new StyleBundle("~/admin/Content/Styles/plugins/css").Include(
            "~/Content/Styles/plugins/plugins.css",
            "~/Content/Styles/plugins/multi-select.css",
            "~/Content/Styles/plugins/bootstrap-timepicker.min.css",
            "~/Content/Styles/plugins/dataTables.bootstrap.css",
            "~/Content/Styles/plugins/datepicker.css",
            "~/Content/Styles/plugins/multi-select.css",
            "~/Content/Styles/plugins/select2.css",
            "~/Content/Styles/plugins/toastr.min.css",
            "~/Content/Styles/plugins/bootstrap-wysihtml5.css",
            "~/Content/Styles/plugins/daterangepicker-bs3.css"
         
            ));

            //  base
            bundles.Add(new ScriptBundle("~/base/bundles/jquery").Include(
            "~/Scripts/jquery-1.7.1.min.js",
            "~/Scripts/plugins/jquery.nivo.slider.pack.js",
            "~/Scripts/plugins/jquery.jcarousel.min.js",
            "~/Scripts/plugins/jquery.colorbox-min.js",
            "~/Scripts/plugins/tabs.js",
            "~/Scripts/plugins/jquery.easing-1.3.min.js",
            "~/Scripts/plugins/cloud_zoom.js",
           // "~/Scripts/themes/bigshop/custom.js",
            "~/Scripts/plugins/jquery.dcjqaccordion.js",
            "~/Scripts/plugins/simpleCart.js",
            "~/Scripts/plugins/jquery.toastmessage-min.js"
          
            ));

            bundles.Add(new StyleBundle("~/base/Content/Styles/theme/css").Include(
                  "~/Content/Styles/themes/bigshop/stylesheet.css"
                  ));

            bundles.Add(new StyleBundle("~/base/Content/Styles/plugins/css").Include(
          
            "~/Content/Styles/plugins/colorbox.css",
            "~/Content/Styles/plugins/slideshow.css",
            "~/Content/Styles/plugins/carousel.css",
            "~/Content/Styles/plugins/valo-elements.css",
            "~/Content/Styles/plugins/jquery.toastmessage-min.css"
             ));

            bundles.Add(new StyleBundle("~/base/Content/Styles/font/css").Include(
            "~/Content/Styles/font-awesome/font-awesome.css"
             ));

           


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            //BundleTable.EnableOptimizations = true;
        }
    }
}
