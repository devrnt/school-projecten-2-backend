using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Filters {
    public class SessieFilter : ActionFilterAttribute {
        private readonly ISessieRepository _sessieRepository;

        public SessieFilter(ISessieRepository sessieRepository) {
            _sessieRepository = sessieRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context) {

            string sessieCode = (context.Controller as Controller).ViewData["sessieCode"] as string;
            string doelgroep = (context.Controller as Controller).ViewData["Doelgroep"] as string;
            if (sessieCode != null) {
                context.HttpContext.Session.SetString("sessieCode", sessieCode);
                context.HttpContext.Session.SetString("Doelgroep", doelgroep);
            }
            base.OnActionExecuted(context);
        }
    }
}
