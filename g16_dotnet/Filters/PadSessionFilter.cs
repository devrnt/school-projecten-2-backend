using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            Pad pad = null;
            if (context.Session.GetString("pad") == null)
            {
                Oefening oefening = new Oefening("Opgave 1", "abc");
                GroepsBewerking groepsBewerking = new GroepsBewerking("def");
                string toegangsCode = "xyz";
                Opdracht testOpdracht = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 1 };
                Opdracht testOpdracht2 = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 2 };
                Actie testActie = new Actie("Ga naar afhaalchinees", testOpdracht);
                pad = new Pad(new List<Opdracht> { testOpdracht, testOpdracht2 }, new List<Actie> { testActie }) { PadId = 1 }; 
            } else
            {
                pad = JsonConvert.DeserializeObject<Pad>(context.Session.GetString("pad"));
            }
            return pad;
        }

        private void WriteCartToSession(Pad pad, HttpContext context)
        {
            context.Session.SetString("pad", JsonConvert.SerializeObject(pad));
        }
    }
}
