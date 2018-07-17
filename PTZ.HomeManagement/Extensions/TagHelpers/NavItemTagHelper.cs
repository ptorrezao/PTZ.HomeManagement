using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Extentions.TagHelpers
{
    [HtmlTargetElement("nav-item")]
    public class NavItemTagHelper : AnchorTagHelper
    {
        public NavItemTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        [HtmlAttributeName("tld-icon")]
        public string icon { get; set; }

        [HtmlAttributeName("tld-initials")]
        public string initials { get; set; }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var childContent = await output.GetChildContentAsync();
            string content = childContent.GetContent();

            var _icon = "";
            if (!string.IsNullOrEmpty(icon))
            {
                _icon = $"<i class='nc-icon {icon}'></i><p>{content}</p>";
            }
            else
            {
                _icon = $"<span class='sidebar-mini'>{initials}</span><span class='sidebar-normal'>{content}</span>";
            }

            output.TagName = "li";
            var hrefAttr = output.Attributes.FirstOrDefault(a => a.Name == "href");
            if (hrefAttr != null)
            {
                output.Content.SetHtmlContent($@"<a class='nav-link' href='{hrefAttr.Value}'>{_icon}</a>");
                output.Attributes.Remove(hrefAttr);
            }
            else
                output.Content.SetHtmlContent(content);

            MakeNavItem(output);

            if (ShouldBeActive())
            {
                MakeActive(output);
            }
        }

        private static void MakeNavItem(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "nav-item");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("nav-item") < 0)
            {
                output.Attributes.SetAttribute("class", classAttr.Value == null
                    ? "nav-item"
                    : classAttr.Value.ToString() + " nav-item");
            }
        }

        private bool ShouldBeActive()
        {
            string currentController = ViewContext.RouteData.Values["Controller"].ToString();
            string currentAction = ViewContext.RouteData.Values["Action"].ToString();

            if (!string.IsNullOrWhiteSpace(Controller) && Controller.ToLower() != currentController.ToLower())
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Action) && Action.ToLower() != currentAction.ToLower())
            {
                return false;
            }

            foreach (KeyValuePair<string, string> routeValue in RouteValues)
            {
                if (!ViewContext.RouteData.Values.ContainsKey(routeValue.Key) || ViewContext.RouteData.Values[routeValue.Key].ToString() != routeValue.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("active") < 0)
            {
                output.Attributes.SetAttribute("class", classAttr.Value == null
                    ? "active"
                    : classAttr.Value.ToString() + " active");
            }
        }

    }
}
