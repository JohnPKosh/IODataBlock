using System.Web.Optimization;

//using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace WebTrackr
{
    /// <summary>
    /// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        /// </summary>
        /// <param name="bundles">The bundles collection to register.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            var cssTransformer = new StyleTransformer();
            var jsTransformer = new ScriptTransformer();
            var nullOrderer = new NullOrderer();

            var cssBundle = new StyleBundle("~/bundles/css");
            cssBundle.Include("~/Content/mvc-site.less", "~/Content/bootstrap/bootstrap.less");
            cssBundle.Transforms.Add(cssTransformer);
            cssBundle.Orderer = nullOrderer;
            bundles.Add(cssBundle);

            var jqueryBundle = new ScriptBundle("~/bundles/jquery");
            jqueryBundle.Include("~/Scripts/jquery-{version}.js");
            jqueryBundle.Transforms.Add(jsTransformer);
            jqueryBundle.Orderer = nullOrderer;
            bundles.Add(jqueryBundle);

            var jqueryvalBundle = new ScriptBundle("~/bundles/jqueryval");
            jqueryvalBundle.Include("~/Scripts/jquery.validate*");
            jqueryvalBundle.Transforms.Add(jsTransformer);
            jqueryvalBundle.Orderer = nullOrderer;
            bundles.Add(jqueryvalBundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            var modernizrBundle = new ScriptBundle("~/bundles/modernizr");
            modernizrBundle.Include("~/Scripts/modernizr-*");
            modernizrBundle.Transforms.Add(jsTransformer);
            modernizrBundle.Orderer = nullOrderer;
            bundles.Add(modernizrBundle);

            var bootstrapBundle = new ScriptBundle("~/bundles/bootstrap");
            bootstrapBundle.Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js");
            bootstrapBundle.Transforms.Add(jsTransformer);
            bootstrapBundle.Orderer = nullOrderer;
            bundles.Add(bootstrapBundle);

            /* Template imported bundles */

            // Template style
            //bundles.Add(new StyleBundle("~/bundles/admin-styles/css").Include(
            //          "~/Content/style.css", new CssRewriteUrlTransform()));

            var cssAdminStyleBundle = new StyleBundle("~/bundles/admin-styles/css");
            cssAdminStyleBundle.Include("~/Content/admin-styles.min.css");
            cssAdminStyleBundle.Transforms.Add(new StyleTransformer());
            cssAdminStyleBundle.Orderer = nullOrderer;
            bundles.Add(cssAdminStyleBundle);

            // Template script
            bundles.Add(new ScriptBundle("~/bundles/admin-scripts/js").Include(
                      "~/Content/metisMenu/dist/metisMenu.min.js",
                      "~/Content/iCheck/icheck.min.js",
                      "~/Content/peity/jquery.peity.min.js",
                      "~/Content/sparkline/index.js",
                      "~/Scripts/admin-dashboard.js",
                      "~/Scripts/charts.js"));

            // Template script mvc admin-only
            bundles.Add(new ScriptBundle("~/bundles/admin-only/js").Include(
                "~/Scripts/admin-mvc.js"
                ));

            // Animate.css
            bundles.Add(new StyleBundle("~/bundles/animate/css").Include(
                      "~/Content/animate.css/animate.min.css"));

            // Pe-icon-7-stroke
            bundles.Add(new StyleBundle("~/bundles/peicon7stroke/css").Include(
                      "~/Icons/pe-icon-7-stroke/css/pe-icon-7-stroke.css", new CssRewriteUrlTransform()));

            // Font Awesome icons style
            bundles.Add(new StyleBundle("~/bundles/font-awesome/css").Include(
                      "~/Content/fontawesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            //// Bootstrap style
            //bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
            //          "~/Content/bootstrap/dist/css/bootstrap.min.css", new CssRewriteUrlTransform()));

            //// Bootstrap
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
            //          "~/Content/bootstrap/dist/js/bootstrap.min.js"));

            //// jQuery
            //bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
            //          "~/Content/jquery/dist/jquery.min.js"));

            //// jQuery UI
            //bundles.Add(new ScriptBundle("~/bundles/jqueryui/js").Include(
            //          "~/Content/jquery-ui/jquery-ui.min.js"));

            // jQuery UI
            bundles.Add(new ScriptBundle("~/bundles/jqueryui/js").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js"));

            // Flot chart
            bundles.Add(new ScriptBundle("~/bundles/flot/js").Include(
                      "~/Content/flot/jquery.flot.js",
                      "~/Content/flot/jquery.flot.tooltip.min.js",
                      "~/Content/flot/jquery.flot.resize.js",
                      "~/Content/flot/jquery.flot.pie.js",
                      "~/Content/flot.curvedlines/curvedLines.js",
                      "~/Content/jquery.flot.spline/index.js"));

            // Star rating
            bundles.Add(new ScriptBundle("~/bundles/starRating/js").Include(
                      "~/Content/bootstrap-star-rating/js/star-rating.min.js"));

            // Star rating style
            bundles.Add(new StyleBundle("~/bundles/starRating/css").Include(
                      "~/Content/bootstrap-star-rating/css/star-rating.min.css", new CssRewriteUrlTransform()));

            // Sweetalert
            bundles.Add(new ScriptBundle("~/bundles/sweetAlert/js").Include(
                      "~/Content/sweetalert/lib/sweet-alert.min.js"));

            // Sweetalert style
            bundles.Add(new StyleBundle("~/bundles/sweetAlert/css").Include(
                      "~/Content/sweetalert/lib/sweet-alert.css"));

            // Toastr
            bundles.Add(new ScriptBundle("~/bundles/toastr/js").Include(
                      "~/Content/toastr/build/toastr.min.js"));

            // Toastr style
            bundles.Add(new StyleBundle("~/bundles/toastr/css").Include(
                      "~/Content/toastr/build/toastr.min.css"));

            // Nestable
            bundles.Add(new ScriptBundle("~/bundles/nestable/js").Include(
                      "~/Content/nestable/jquery.nestable.js"));

            // Toastr
            bundles.Add(new ScriptBundle("~/bundles/bootstrapTour/js").Include(
                      "~/Content/bootstrap-tour/build/js/bootstrap-tour.min.js"));

            // Toastr style
            bundles.Add(new StyleBundle("~/bundles/bootstrapTour/css").Include(
                      "~/Content/bootstrap-tour/build/css/bootstrap-tour.min.css"));

            // Moment
            bundles.Add(new ScriptBundle("~/bundles/moment/js").Include(
                      "~/Content/moment/moment.js"));

            // Full Calendar
            bundles.Add(new ScriptBundle("~/bundles/fullCalendar/js").Include(
                      "~/Content/fullcalendar/dist/fullcalendar.min.js"));

            // Full Calendar style
            bundles.Add(new StyleBundle("~/bundles/fullCalendar/css").Include(
                      "~/Content/fullcalendar/dist/fullcalendar.min.css"));

            // Chart JS
            bundles.Add(new ScriptBundle("~/bundles/chartjs/js").Include(
                      "~/Content/chartjs/Chart.min.js"));

            // Datatables
            bundles.Add(new ScriptBundle("~/bundles/datatables/js").Include(
                      "~/Content/datatables/media/js/jquery.dataTables.min.js"));

            // Datatables bootstrap
            bundles.Add(new ScriptBundle("~/bundles/datatablesBootstrap/js").Include(
                      "~/Content/datatables_plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"));

            // Datatables style
            bundles.Add(new StyleBundle("~/bundles/datatables/css").Include(
                      "~/Content/datatables_plugins/integration/bootstrap/3/dataTables.bootstrap.css"));

            // Xeditable
            bundles.Add(new ScriptBundle("~/bundles/xeditable/js").Include(
                      "~/Content/xeditable/bootstrap3-editable/js/bootstrap-editable.min.js"));

            // Xeditable style
            bundles.Add(new StyleBundle("~/bundles/xeditable/css").Include(
                      "~/Content/xeditable/bootstrap3-editable/css/bootstrap-editable.css", new CssRewriteUrlTransform()));

            // Select 2
            bundles.Add(new ScriptBundle("~/bundles/select2/js").Include(
                      "~/Content/select2-3.5.2/select2.min.js"));

            // Select 2 style
            bundles.Add(new StyleBundle("~/bundles/select2/css").Include(
                      "~/Content/select2-3.5.2/select2.css",
                      "~/Content/select2-bootstrap/select2-bootstrap.css"));

            // Touchspin
            bundles.Add(new ScriptBundle("~/bundles/touchspin/js").Include(
                      "~/Content/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.js"));

            // Touchspin style
            bundles.Add(new StyleBundle("~/bundles/touchspin/css").Include(
                      "~/Content/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.css"));

            // Datepicker
            bundles.Add(new ScriptBundle("~/bundles/datepicker/js").Include(
                      "~/Content/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"));

            // Datepicker style
            bundles.Add(new StyleBundle("~/bundles/datepicker/css").Include(
                      "~/Content/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css"));

            // Datepicker
            bundles.Add(new ScriptBundle("~/bundles/summernote/js").Include(
                      "~/Content/summernote/dist/summernote.min.js"));

            // Datepicker style
            bundles.Add(new StyleBundle("~/bundles/summernote/css").Include(
                      "~/Content/summernote/dist/summernote.css",
                      "~/Content/summernote/dist/summernote-bs3.css"));

            // Bootstrap checkbox style
            bundles.Add(new StyleBundle("~/bundles/bootstrapCheckbox/css").Include(
                      "~/Content/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css"));

            // Blueimp gallery
            bundles.Add(new ScriptBundle("~/bundles/blueimp/js").Include(
                      "~/Content/blueimp-gallery/js/jquery.blueimp-gallery.min.js"));

            // Blueimp gallery style
            bundles.Add(new StyleBundle("~/bundles/blueimp/css").Include(
                      "~/Content/blueimp-gallery/css/blueimp-gallery.min.css", new CssRewriteUrlTransform()));
        }

        //// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                "~/Scripts/jquery.validate*"));

        //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
        //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js",
        //              "~/Scripts/respond.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/site.css"));
        //}
    }
}