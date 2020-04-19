using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace T2Access.Web.Helper
{
    public static class HtmlHelperExtensions
    {

        public static string IsSelected(this IHtmlHelper html, string controllers = "", string actions = "", string cssClass = "selected")
        {
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.ClientValidationEnabled;

            if (isChildAction)
                viewContext = html.ViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(actions))
                actions = currentAction;

            if (string.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : string.Empty;
        }




        public static IHtmlContent EnumDropDownListWithStringFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            var selectListItem = Enum.GetNames(Nullable.GetUnderlyingType(typeof(TEnum))).Select(p => new SelectListItem() { Value = p, Text = p }).ToList();
            return HtmlHelperSelectExtensions.DropDownListFor(htmlHelper, expression, selectListItem, htmlAttributes);
        }


    }
}