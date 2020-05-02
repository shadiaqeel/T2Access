using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace T2Access.Web.SPA.VueJs.Helpers
{
    public static class ModelStateHelper
    {
        public static string GetModelStateErrors(this ModelStateDictionary modelState)
        {
            IEnumerable<KeyValuePair<string, string[]>> errors = modelState.IsValid
                ? null
                : modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());

            string output = "{";

            if (errors != null)
            {
                foreach (KeyValuePair<string, string[]> kvp in errors)
                {
                    output += "\"" + kvp.Key.Replace(".", "") + "\":\"" + String.Join(", ", kvp.Value) + "\",";
                }
            }
            output = output.TrimEnd(',');
            output += "}";
            return output;
        }
    }
}
