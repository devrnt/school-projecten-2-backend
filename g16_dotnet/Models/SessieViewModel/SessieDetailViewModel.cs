using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace g16_dotnet.Models.SessieViewModel
{
    public class SessieDetailViewModel
    {
        [Display(Name = "Sessie code")]
        public int SessieCode { get; set; }
        [Display(Name = "Sessie naam")]
        public string SessieNaam { get; set; }
        public IEnumerable<Groep> Groepen { get; set; }

        public SessieDetailViewModel(Sessie sessie)
        {
            SessieNaam = sessie.Naam;
            SessieCode = sessie.SessieCode;
            Groepen = sessie.Groepen;
        }
    }
}
