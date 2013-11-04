namespace Handlebars.Web
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Caching;
    using System.Web.Mvc;
    using Handlebars.Core;

    public class HandlebarsView : IView
    {
        private readonly HandlebarsViewEngine engine;
        private readonly ControllerContext controllerContext;
        private readonly string viewPath;
        private readonly string masterPath;

        public HandlebarsView (HandlebarsViewEngine engine, ControllerContext controllerContext, string viewPath, string masterPath)
        {
            this.engine = engine;
            this.controllerContext = controllerContext;
            this.viewPath = viewPath;
            this.masterPath = masterPath;
        }

        public void Render (ViewContext viewContext, TextWriter writer)
        {
            var data = engine.RootContext == HandlebarsViewEngineRootContext.ViewData
                           ? viewContext.ViewData
                           : viewContext.ViewData.Model;

            Template template = string.IsNullOrEmpty (masterPath)
                                    ? LoadTemplate (viewPath)
                                    : LoadTemplate (masterPath);

            if (template != null)
            {
                template.Render (writer, data, FindPartial);
            }
        }

        private Template GetTemplate ()
        {
            return LoadTemplate (viewPath);
        }

        private Template LoadTemplate (string path)
        {
            if (controllerContext.HttpContext.Cache [path] != null)
            {
                return (Template) controllerContext.HttpContext.Cache [path];
            }

            var templatePath = controllerContext.HttpContext.Server.MapPath (path);
            var templateSource = File.ReadAllText (templatePath);

            var compiler = new Compiler ();
            var template = compiler.Compile (templateSource);

            controllerContext.HttpContext.Cache.Insert (path, template, new CacheDependency (templatePath));

            return template;
        }

        private Template FindPartial (string name)
        {
            var viewResult = engine.FindPartialView (controllerContext, name, false);

            if (viewResult != null)
            {
                if (viewResult.View == null)
                {
                    var builder = new StringBuilder ();

                    foreach (var str in viewResult.SearchedLocations)
                    {
                        builder.AppendLine ();
                        builder.Append (str);
                    }

                    var msg = string.Format (
                        "The partial view '{0}' was not found or no view engine supports the searched locations. The following locations were searched:{1}",
                        name,
                        builder);

                    throw new InvalidOperationException (msg);
                }

                var handlebarsView = viewResult.View as HandlebarsView;
                if (handlebarsView != null)
                {
                    return handlebarsView.GetTemplate ();
                }
            }

            return null;
        }

    }
}