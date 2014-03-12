using System.IO;
using System.Web.Mvc;

namespace DaringCore.Web
{
    /// <summary>
    /// Why -- This class is very useful for emails as it lets you build an 
    ///         email template as a view, using partial views and layout views
    ///         which can be then generated to strings and passed directly as
    ///         an email body.
    /// 
    /// Ussage:
    ///     Static:
    ///         string renderedView = ViewRenderer.RenderView("~/Views/MyController/Index.cshtml", model, controllerContext);
    ///     
    ///     else
    ///     
    ///         var renderer = new ViewRenderer(controllerContext);
    ///         string renderedView = ViewRenderer.RenderView("~/Views/MyController/Index.cshtml", model, controllerContext);
    /// </summary>
    public class RenderViewsToString
    {
        /// <summary>
        /// Required Controller Context
        /// </summary>
        protected ControllerContext Context { get; set; }

        public RenderViewsToString(ControllerContext controllerContext)
        {
            Context = controllerContext;
        }

        /// <summary>
        /// Renders a full MVC view to a string. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout        
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to render the view with</param>
        /// <returns>String of the rendered view or null on error</returns>
        public string RenderView(string viewPath, object model)
        {
            return RenderViewToStringInternal(viewPath, model, false);
        }

        /// <summary>
        /// Renders a partial MVC view to string. Use this method to render
        /// a partial view that doesn't merge with _Layout and doesn't fire
        /// _ViewStart.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <returns>String of the rendered view or null on error</returns>
        public string RenderPartialView(string viewPath, object model)
        {
            return RenderViewToStringInternal(viewPath, model, true);
        }

        /// <summary>
        /// Renders a full MVC view to a string. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout        
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to render the view with</param>
        /// <param name="controllerContext">The current controller context of the request</param>
        /// <returns>String of the rendered view or null on error</returns>
        public static string RenderView(string viewPath, object model,
                                        ControllerContext controllerContext)
        {
            var renderer = new RenderViewsToString(controllerContext);
            return renderer.RenderView(viewPath, model);
        }

        /// <summary>
        /// Renders a partial MVC view to string. Use this method to render
        /// a partial view that doesn't merge with _Layout and doesn't fire
        /// _ViewStart.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="controllerContext">The current controller context of the request</param>
        /// <returns>String of the rendered view or null on error</returns>
        public static string RenderPartialView(string viewPath, object model,
                                               ControllerContext controllerContext)
        {
            var renderer = new RenderViewsToString(controllerContext);
            return renderer.RenderPartialView(viewPath, model);
        }

        protected string RenderViewToStringInternal(string viewPath, object model,
                                                    bool partial = false)
        {
            ViewEngineResult viewEngineResult;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(Context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(Context, viewPath, null);

            if (viewEngineResult == null || viewEngineResult.View == null)
                throw new FileNotFoundException("View could not be found.");

            var view = viewEngineResult.View;
            Context.Controller.ViewData.Model = model;

            string result;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(Context, view,
                                            Context.Controller.ViewData,
                                            Context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}
