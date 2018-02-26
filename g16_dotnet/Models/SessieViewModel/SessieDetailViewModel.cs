using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace g16_dotnet.Models.SessieViewModel
{
    public class SessieDetailViewModel
    {
        [HiddenInput]
        public int SessieId { get; set; }
        [Display(Name = "Sessie naam")]
        public string SessieNaam { get; set; }
        [Display(Name = "Sessie code")]
        public int SessieCode { get; set; }
        public IEnumerable<Groep> Groepen { get; set; }

        public SessieDetailViewModel(Sessie sessie)
        {
            SessieId = sessie.SessieId;
            SessieNaam = sessie.Naam;
            SessieCode = sessie.SessieCode;
            Groepen = sessie.Groepen;
        }
    }
}
