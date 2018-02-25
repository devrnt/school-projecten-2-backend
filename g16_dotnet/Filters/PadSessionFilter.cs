using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace g16_dotnet.Filters {
    public class PadSessionFilter : ActionFilterAttribute {
        private Pad _pad;
        private readonly IPadRepository _padRepository;
        private readonly IOpdrachtRepository _opdrachtRepository;
        private readonly IActieRepository _actieRepository;
        private readonly IGroepRepository _groepRepository;

        public PadSessionFilter(IPadRepository padRepository, IOpdrachtRepository opdrachtRepository, IActieRepository actieRepository, IGroepRepository groepRepository) {
            _padRepository = padRepository;
            _opdrachtRepository = opdrachtRepository;
            _actieRepository = actieRepository;
            _groepRepository = groepRepository;

        }
        public override void OnActionExecuting(ActionExecutingContext context) {
            _pad = ReadPadFromSession(context.HttpContext);
            context.ActionArguments["pad"] = _pad;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            WritePadToSession(_pad, context.HttpContext);
            base.OnActionExecuted(context);
        }

        private Pad ReadPadFromSession(HttpContext context) {
            Pad pad = null;
            if (context.Session.GetString("pad") == null) {
                var padId = _groepRepository.GetById(1).Pad.PadId;
                pad = _padRepository.GetById(padId);

                //Oefening oefening = new Oefening("Opgave 1", "abc");
                //GroepsBewerking groepsBewerking = new GroepsBewerking("def");
                //string toegangsCode = "xyz";
                //Opdracht testOpdracht = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 1 };
                //Opdracht testOpdracht2 = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 2 };
                //Actie testActie = new Actie("Ga naar afhaalchinees", testOpdracht);
                //pad = new Pad(new List<Opdracht> { testOpdracht, testOpdracht2 }, new List<Actie> { testActie }) { PadId = 1 };
            } else {
                pad = JsonConvert.DeserializeObject<Pad>(context.Session.GetString("pad"));
                foreach(var opdracht in pad.Opdrachten)
                {
                    Opdracht o = _opdrachtRepository.GetById(opdracht.VolgNr);
                    opdracht.Oefening = o.Oefening;
                    opdracht.GroepsBewerking = o.GroepsBewerking;
                    opdracht.ToegangsCode = o.ToegangsCode;
                }
                foreach(var actie in pad.Acties)
                {
                    Actie a = _actieRepository.GetById(actie.ActieId);
                    actie.GelinkteOpdracht = a.GelinkteOpdracht;
                    actie.Omschrijving = a.Omschrijving;
                }
            }
            return pad;
        }

        private void WritePadToSession(Pad pad, HttpContext context) {
            context.Session.SetString("pad", JsonConvert.SerializeObject(pad));
            _actieRepository.SaveChanges();
            _opdrachtRepository.SaveChanges();
            _padRepository.SaveChanges();
        }
    }
}
