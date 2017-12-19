using AEFWeb.Api.Security;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEFWeb.Api.Filters
{
    public class TokenAddFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dic = filterContext.ActionArguments.FirstOrDefault();
            var defaultValue = default(KeyValuePair<string, object>);

            if (!dic.Equals(defaultValue))
            {
                dynamic model = dic.Value;
                model = SetValues(filterContext, model);
            }
        }

        private dynamic SetValues(ActionExecutingContext filterContext, dynamic model)
        {
            var path = filterContext.HttpContext.Request.Path.Value;
            var split = path.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (split.LastOrDefault().Contains("add"))
            {
                try
                {
                    model.CreatorUserId = ManageToken.GetToken(filterContext.HttpContext.Request);
                }
                catch { }
                try
                {
                    model.CreationDate = DateTime.Now;
                }
                catch { }
            }
            return model;
        }
    }
}
