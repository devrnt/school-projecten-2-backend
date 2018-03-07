using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Filters {
    public class PadFilter : ActionFilterAttribute {
        private Pad _pad;
        private readonly IPadRepository _padRepository;

        public PadFilter(IPadRepository padRepository) {
            _padRepository = padRepository;
        }
        public override void OnActionExecuting(ActionExecutingContext context) {
            // na implementeren van users zal dit pad gehaald worden adhv de ingelogde groep
            _pad = _padRepository.GetById(1);
            if (_pad.IsGeblokkeerd)
                _pad.PadState = new GeblokkeerdPadState("Geblokkeerd");
            else if (_pad.Voortgang == _pad.AantalOpdrachten)
                _pad.PadState = new SchatkistPadState("Schatkist");
            else if (_pad.Voortgang <= _pad.Acties.Count(a => a.Actie.IsUitgevoerd))
                _pad.PadState = new OpdrachtPadState("Opdracht");
            else
                _pad.PadState = new ActiePadState("Actie");
            context.ActionArguments["pad"] = _pad;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            _padRepository.SaveChanges();
            base.OnActionExecuted(context);
        }


// Deze code is enkel nodig indien we dienen gebruik te maken van Session storage
        //private Pad ReadPadFromSession(HttpContext context) {
        //    Pad pad = null;
        //    if (context.Session.GetString("pad") == null) {
        //        var padId = _groepRepository.GetById(1).Pad.PadId;
        //        pad = _padRepository.GetById(padId);

        //        //Oefening oefening = new Oefening("Opgave 1", "abc");
        //        //GroepsBewerking groepsBewerking = new GroepsBewerking("def");
        //        //string toegangsCode = "xyz";
        //        //Opdracht testOpdracht = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 1 };
        //        //Opdracht testOpdracht2 = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 2 };
        //        //Actie testActie = new Actie("Ga naar afhaalchinees", testOpdracht);
        //        //pad = new Pad(new List<Opdracht> { testOpdracht, testOpdracht2 }, new List<Actie> { testActie }) { PadId = 1 };
        //    } else {
        //        pad = JsonConvert.DeserializeObject<Pad>(context.Session.GetString("pad"));
        //        foreach(var opdracht in pad.Opdrachten)
        //        {
        //            Opdracht o = _opdrachtRepository.GetById(opdracht.VolgNr);
        //            opdracht.Oefening = o.Oefening;
        //            opdracht.GroepsBewerking = o.GroepsBewerking;
        //            opdracht.ToegangsCode = o.ToegangsCode;
        //        }
        //        foreach(var actie in pad.Acties)
        //        {
        //            Actie a = _actieRepository.GetById(actie.ActieId);
        //            actie.GelinkteOpdracht = a.GelinkteOpdracht;
        //            actie.Omschrijving = a.Omschrijving;
        //        }
        //    }
        //    return pad;
        //}

        //private void WritePadToSession(Pad pad, HttpContext context) {
        //    context.Session.SetString("pad", JsonConvert.SerializeObject(pad));
        //    _actieRepository.SaveChanges();
        //    _opdrachtRepository.SaveChanges();
        //    _padRepository.SaveChanges();
        //}
    }
}
