namespace Handlebars.Web
{
    using System.Web.Mvc;

    public class HandlebarsViewEngine : VirtualPathProviderViewEngine
    {
        public HandlebarsViewEngine (string [] fileExtensions = null)
        {
            // If we're using MVC, we probably want to use the same encoder MVC uses.
            //Encoders.HtmlEncode = HttpUtility.HtmlEncode;

            FileExtensions = fileExtensions ?? new []
                                               {
                                                   "hb"
                                               };
            SetLocationFormats ();
            RootContext = HandlebarsViewEngineRootContext.Model;
        }

        public static void Register (ViewEngineCollection engines)
        {
            engines.RemoveAt (0);
            engines.Add (new HandlebarsViewEngine ());
        }

        public static void Register ()
        {
            Register (ViewEngines.Engines);
        }

        private void SetLocationFormats ()
        {
            var fileExtension = FileExtensions [0];

            MasterLocationFormats = new []
                                    {
                                        "~/Views/{1}/{0}." + fileExtension,
                                        "~/Views/Shared/{0}." + fileExtension
                                    };
            ViewLocationFormats = new []
                                  {
                                      "~/Views/{1}/{0}." + fileExtension,
                                      "~/Views/Shared/{0}." + fileExtension
                                  };
            PartialViewLocationFormats = new []
                                         {
                                             "~/Views/{1}/{0}." + fileExtension,
                                             "~/Views/Shared/{0}." + fileExtension
                                         };
            AreaMasterLocationFormats = new []
                                        {
                                            "~/Areas/{2}/Views/{1}/{0}." + fileExtension,
                                            "~/Areas/{2}/Views/Shared/{0}." + fileExtension
                                        };
            AreaViewLocationFormats = new []
                                      {
                                          "~/Areas/{2}/Views/{1}/{0}." + fileExtension,
                                          "~/Areas/{2}/Views/Shared/{0}." + fileExtension
                                      };
            AreaPartialViewLocationFormats = new []
                                             {
                                                 "~/Areas/{2}/Views/{1}/{0}." + fileExtension,
                                                 "~/Areas/{2}/Views/Shared/{0}." + fileExtension
                                             };
        }

        public HandlebarsViewEngineRootContext RootContext
        {
            get;
            set;
        }

        protected override IView CreateView (ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return GetView (controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView (ControllerContext controllerContext, string partialPath)
        {
            return GetView (controllerContext, partialPath, null);
        }

        private IView GetView (ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new HandlebarsView (this, controllerContext, viewPath, masterPath);
        }
    }
}