using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor;
using PTZ.HomeManagement.MyFinance;

namespace PTZ.HomeManagement.Utils
{
    public static class HtmlExtensions
    {
        public static HtmlString Script(this IHtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return HtmlString.Empty;
        }

        public static IHtmlContent RenderScripts(this IHtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return HtmlString.Empty;
        }
        public static string GetColor(this Bank bank)
        {
            switch (bank)
            {
                case Bank.CGD:
                    return "rgba(40, 90, 155, 0.6)";
                case Bank.BPI:
                    return "rgba(253, 110, 9, 0.6)";
                default:
                    return "#4cff00";
            }
        }

        public static string ConvertToCurrency(this decimal value, string currency)
        {
            return value.ToString("#,##0.00;-#,##0.00;0.00}") + " " + currency;
        }

        public static string ConvertToPercentage(this decimal value)
        {
            return value.ToString("#,##0.00;-#,##0.00;0.00}") + "%";
        }
    }

}
