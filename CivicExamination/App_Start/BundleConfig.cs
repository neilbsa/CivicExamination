using System.Web;
using System.Web.Optimization;

namespace CivicExamination
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery.form.min.js",
                           "~/Scripts/jquery.validate.min.js",
                           "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/CustomScripts/BaseScripts/BaseScripts.js",

                           "~/Scripts/jquery-ui-{version}.js"
                           ));

            bundles.Add(new ScriptBundle("~/bundles/BoostrapCalendarScript").Include(
                   "~/Scripts/bootstrap-year-calendar/js/bootstrap-year-calendar.js"

                                ));

            bundles.Add(new ScriptBundle("~/bundles/BootstrapMaterialDatetimePicker").Include(
                "~/Scripts/moment.min.js",
                 "~/Scripts/bootstrap-material-datepicker/js/bootstrap-material-datetimepicker.js"

                              ));


            bundles.Add(new StyleBundle("~/Content/BootstrapMaterialDatetimePickerCss").Include(
             "~/Scripts/bootstrap-material-datepicker/css/bootstrap-material-datetimepicker.css"

                          ));



            bundles.Add(new ScriptBundle("~/bundles/TimeCircle").Include(
                 "~/Scripts/inc/TimeCircles.js"

                              ));


            bundles.Add(new ScriptBundle("~/bundles/MaterializeScript").Include(
                    "~/Content/mdl-v1.1.2/material.min.js"


                 ));


            bundles.Add(new ScriptBundle("~/bundles/BootStrap").Include(
                       "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/AccountModule").Include(
                    "~/Scripts/CustomScripts/Account/AccountScripts.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/EvaluationModule").Include(
                    "~/Scripts/CustomScripts/Evaluation/EvaluationAdmin.js",
                    "~/Scripts/CustomScripts/Evaluation/Evaluator.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/Category").Include(
                "~/Scripts/CustomScripts/category/Category.js"

             ));
            bundles.Add(new ScriptBundle("~/bundles/MainExam").Include(
              "~/Scripts/CustomScripts/MainExamination/MainExamination.js"

           ));

            bundles.Add(new ScriptBundle("~/bundles/JobPosting").Include(
          "~/Scripts/CustomScripts/JobPosting/JobPosting.js"

       ));

            bundles.Add(new ScriptBundle("~/bundles/OnExam").Include(
        "~/Scripts/CustomScripts/MainExamination/OnExamination.js"

     ));
            bundles.Add(new ScriptBundle("~/bundles/Communication").Include(
        "~/Scripts/jquery.signalR-2.3.0.min.js",
           "~/Scripts/CustomScripts/Comms/MainComm.js"
        

  ));

            bundles.Add(new ScriptBundle("~/bundles/Examination").Include(
              "~/Scripts/CustomScripts/Examination/Examination.js",
                 "~/Scripts/CustomScripts/Examination/Schedule.js"

           ));


            bundles.Add(new ScriptBundle("~/bundles/Categorydetails").Include(
               "~/Scripts/CustomScripts/category/QuestionDetail.js"
             ));
            bundles.Add(new ScriptBundle("~/bundles/Questiondetails").Include(
            "~/Scripts/CustomScripts/category/QuestionScripts.js"
          ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                        "~/Scripts/bootstrap-year-calendar/bootstrap-year-calendar.css",
                      "~/Content/themes/base/jquery-ui.min.css",

                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/MaterializeCss").Include(
                     "~/Content/mdl-v1.1.2/material.min.css"


                        ));



            bundles.Add(new StyleBundle("~/Content/BoostrapCalendarCss").Include(
                  "~/Scripts/bootstrap-year-calendar/css/bootstrap-year-calendar.css"

                      ));

        }




    }
}
