﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BootstrapForms.Utilities
{
    /// <summary>
    /// Server-side renders
    /// </summary>
    public static class Renders
    {
        /// <summary>
        /// Server-side render partial view
        /// </summary>
        public static string BsRenderPartialView(this ControllerBase controller, string viewName, object model, string htmlFieldPrefix = "")
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            var backup = controller.ViewData.Model;
            controller.ViewData.Model = model;

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

                var backupPrefix = controller.ViewData.TemplateInfo.HtmlFieldPrefix;
                controller.ViewData.TemplateInfo.HtmlFieldPrefix = htmlFieldPrefix;

                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewContext.FormContext = null;
                viewResult.View.Render(viewContext, sw);

                controller.ViewData.TemplateInfo.HtmlFieldPrefix = backupPrefix;
                controller.ViewData.Model = backup;
                return sw.GetStringBuilder().ToString().Trim();
            }
        }

        /// <summary>
        /// Server-side render view
        /// </summary>
        public static string BsRenderView(this ControllerBase controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, null);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString().Trim();
            }
        }
    }
}
