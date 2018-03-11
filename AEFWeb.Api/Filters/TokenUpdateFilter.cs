using AEFWeb.Api.Security;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEFWeb.Api.Filters
{
    public class TokenUpdateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dic = filterContext.ActionArguments.FirstOrDefault();
            var defaultValue = default(KeyValuePair<string, object>);

            if (!dic.Equals(defaultValue))
            {
                dynamic model = dic.Value;
                model = SetPut(filterContext, model);
            }
        }

        private dynamic SetPut(ActionExecutingContext filterContext, dynamic model)
        {
            try
            {
                model.LastUpdatedUserId = ManageToken.GetToken(filterContext.HttpContext.Request);
            }
            catch { }
            try
            {
                model.LastUpdateDate = DateTime.Now;
            }
            catch { }
            return model;
        }
    }
}
