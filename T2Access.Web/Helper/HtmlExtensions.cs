using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace T2Access.Web.Helper
{
    public static class HtmlExtensions
    {

        public static IHtmlString RegisteredScripts(this HtmlHelper htmlHelper)
        {
            var ctx = htmlHelper.ViewContext.HttpContext;
            var registeredScripts = ctx.Items["_scripts_"] as Stack<string>;
            if (registeredScripts == null || registeredScripts.Count < 1)
            {
                return null;
            }
            var sb = new StringBuilder();
            foreach (var script in registeredScripts)
            {
                var scriptBuilder = new TagBuilder("script");
                scriptBuilder.Attributes["type"] = "text/javascript";
                scriptBuilder.Attributes["src"] = script;
                sb.AppendLine(scriptBuilder.ToString(TagRenderMode.Normal));
            }
            return new HtmlString(sb.ToString());
        }

        public static void RegisterScript(this HtmlHelper htmlHelper, string script)
        {
            var ctx = htmlHelper.ViewContext.HttpContext;
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var registeredScripts = ctx.Items["_scripts_"] as Stack<string>;
            if (registeredScripts == null)
            {
                registeredScripts = new Stack<string>();
                ctx.Items["_scripts_"] = registeredScripts;
            }
            var src = urlHelper.Content(script);
            if (!registeredScripts.Contains(src))
            {
                registeredScripts.Push(src);
            }
        }


        public static MvcHtmlString EnumDropDownListWithStringFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            var selectListItem = Enum.GetNames(Nullable.GetUnderlyingType(typeof(TEnum))).Select(p => new SelectListItem() { Value = p, Text = p }).ToList();
            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectListItem, htmlAttributes);
        }


    }
}