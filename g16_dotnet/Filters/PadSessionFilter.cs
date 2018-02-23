using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Filters
{
    public class PadSessionFilter : ActionFilterAttribute
    {
        private Pad _pad;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _pad = ReadPadFromSession(context.HttpContext);
            context.ActionArguments["pad"] = _pad;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            WriteCartToSession(_pad, context.HttpContext);
            base.OnActionExecuted(context);
        }

        private Pad ReadPadFromSession(HttpContext context)
        {
            Pad pad = context.Session.GetString("pad") == null ?
                new Pad() : JsonConvert.DeserializeObject<Pad>(context.Session.GetString("pad"));
            return pad;
        }

        private void WriteCartToSession(Pad pad, HttpContext context)
        {
            context.Session.SetString("pad", JsonConvert.SerializeObject(pad));
        }
    }
}
